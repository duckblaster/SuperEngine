//===== Copyright � 1996-2005, Valve Corporation, All rights reserved. ======//
//
// Purpose: 
//
// $NoKeywords: $
//
//===========================================================================//
#include "cbase.h"
#include "hud.h"
#include "clientmode_sdk.h"
#include "cdll_client_int.h"
#include "iinput.h"
#include "vgui/isurface.h"
#include "vgui/ipanel.h"
#include <vgui_controls/AnimationController.h>
#include "ivmodemanager.h"
#include "BuyMenu.h"
#include "filesystem.h"
#include "vgui/ivgui.h"
#include "hud_basechat.h"
#include "view_shared.h"
#include "view.h"
#include "ivrenderview.h"
#include "model_types.h"
#include "iefx.h"
#include "dlight.h"
#include <imapoverview.h>
#include "c_playerresource.h"
#include <keyvalues.h>
#include "text_message.h"
#include "panelmetaclassmgr.h"
#include "weapon_sdkbase.h"
#include "c_sdk_player.h"
#include "c_weapon__stubs.h"		//Tony; add stubs
#include "sdk_in_main.h"

class CHudChat;

ConVar default_fov( "default_fov", "90", FCVAR_CHEAT );

IClientMode *g_pClientMode = NULL;

//Tony; add stubs for cycler weapon and cubemap.
STUB_WEAPON_CLASS( cycler_weapon,   WeaponCycler,   C_BaseCombatWeapon );
STUB_WEAPON_CLASS( weapon_cubemap,  WeaponCubemap,  C_BaseCombatWeapon );

//-----------------------------------------------------------------------------
// HACK: the detail sway convars are archive, and default to 0.  Existing CS:S players thus have no detail
// prop sway.  We'll force them to DoD's default values for now.  What we really need in the long run is
// a system to apply changes to archived convars' defaults to existing players.
extern ConVar cl_detail_max_sway;
extern ConVar cl_detail_avoid_radius;
extern ConVar cl_detail_avoid_force;
extern ConVar cl_detail_avoid_recover_speed;
// --------------------------------------------------------------------------------- //
// CSDKModeManager.
// --------------------------------------------------------------------------------- //

class CSDKModeManager : public IVModeManager
{
public:
	virtual void	Init();
	virtual void	SwitchMode( bool commander, bool force ) {}
	virtual void	LevelInit( const char *newmap );
	virtual void	LevelShutdown( void );
	virtual void	ActivateMouse( bool isactive ) {}
};

static CSDKModeManager g_ModeManager;
IVModeManager *modemanager = ( IVModeManager * )&g_ModeManager;

// --------------------------------------------------------------------------------- //
// CSDKModeManager implementation.
// --------------------------------------------------------------------------------- //

#define SCREEN_FILE		"scripts/vgui_screens.txt"

void CSDKModeManager::Init()
{
	g_pClientMode = GetClientModeNormal();
	
	PanelMetaClassMgr()->LoadMetaClassDefinitionFile( SCREEN_FILE );
}

void CSDKModeManager::LevelInit( const char *newmap )
{
	g_pClientMode->LevelInit( newmap );
	// HACK: the detail sway convars are archive, and default to 0.  Existing CS:S players thus have no detail
	// prop sway.  We'll force them to DoD's default values for now.
	if ( !cl_detail_max_sway.GetFloat() &&
		!cl_detail_avoid_radius.GetFloat() &&
		!cl_detail_avoid_force.GetFloat() &&
		!cl_detail_avoid_recover_speed.GetFloat() )
	{
		cl_detail_max_sway.SetValue( "5" );
		cl_detail_avoid_radius.SetValue( "64" );
		cl_detail_avoid_force.SetValue( "0.4" );
		cl_detail_avoid_recover_speed.SetValue( "0.25" );
	}

	AB_Input_LevelInit();
}

void CSDKModeManager::LevelShutdown( void )
{
	g_pClientMode->LevelShutdown();
}

//-----------------------------------------------------------------------------
// Purpose: 
//-----------------------------------------------------------------------------
ClientModeSDKNormal::ClientModeSDKNormal()
{
}

//-----------------------------------------------------------------------------
// Purpose: If you don't know what a destructor is by now, you are probably going to get fired
//-----------------------------------------------------------------------------
ClientModeSDKNormal::~ClientModeSDKNormal()
{
}

void ClientModeSDKNormal::Init()
{
	BaseClass::Init();
}

void ClientModeSDKNormal::InitViewport()
{
	m_pViewport = new SDKViewport();
	m_pViewport->Start( gameuifuncs, gameeventmanager );
}

ClientModeSDKNormal g_ClientModeNormal;

IClientMode *GetClientModeNormal()
{
	return &g_ClientModeNormal;
}


ClientModeSDKNormal* GetClientModeSDKNormal()
{
	Assert( dynamic_cast< ClientModeSDKNormal* >( GetClientModeNormal() ) );

	return static_cast< ClientModeSDKNormal* >( GetClientModeNormal() );
}

float ClientModeSDKNormal::GetViewModelFOV( void )
{
	//Tony; retrieve the fov from the view model script, if it overrides it.
	float viewFov = 74.0;

	C_WeaponSDKBase *pWeapon = (C_WeaponSDKBase*)GetActiveWeapon();
	if ( pWeapon )
	{
		viewFov = pWeapon->GetWeaponFOV();
	}
	return viewFov;
}

int ClientModeSDKNormal::GetDeathMessageStartHeight( void )
{
	return m_pViewport->GetDeathMessageStartHeight();
}

void ClientModeSDKNormal::PostRenderVGui()
{
}

bool ClientModeSDKNormal::CanRecordDemo( char *errorMsg, int length ) const
{
	C_SDKPlayer *player = C_SDKPlayer::GetLocalSDKPlayer();
	if ( !player )
	{
		return true;
	}

	if ( !player->IsAlive() )
	{
		return true;
	}

	return true;
}

void ClientModeSDKNormal::OverrideView( CViewSetup *pSetup )
{
	QAngle camAngles;

	// Let the player override the view.
	C_BasePlayer *pPlayer = C_SDKPlayer::GetLocalOrSpectatedPlayer();
	if(!pPlayer)
		return;

	pPlayer->OverrideView( pSetup );

	if( ::input->CAM_IsThirdPerson() )
	{
		Vector cam_ofs;

		::input->CAM_GetCameraOffset( cam_ofs );

		camAngles[ PITCH ] = cam_ofs[ PITCH ];
		camAngles[ YAW ] = cam_ofs[ YAW ];
		camAngles[ ROLL ] = 0;

		Vector camForward, camRight, camUp;
		AngleVectors( camAngles, &camForward, &camRight, &camUp );

		VectorMA( pSetup->origin, -cam_ofs[ ROLL ], camForward, pSetup->origin );

		// Override angles from third person camera
		float flRoll = pSetup->angles.z;
		pSetup->angles = camAngles;
		pSetup->angles.z = flRoll;
	}
}

ConVar m_verticaldamping("m_verticaldamping", "0.85", FCVAR_CLIENTDLL|FCVAR_ARCHIVE, "Multiplier to dampen vertical component of mouse movement.", true, 0.1f, true, 1);
ConVar m_slowmodamping("m_slowmodamping", "0.6", FCVAR_CLIENTDLL|FCVAR_ARCHIVE, "Multiplier to dampen mouse movement during slow motion.", true, 0.1f, true, 1);
ConVar m_aimindamping("m_aimindamping", "0.5", FCVAR_CLIENTDLL|FCVAR_ARCHIVE, "Multiplier to dampen mouse movement during slow motion.", true, 0.1f, true, 1);

void ClientModeSDKNormal::OverrideMouseInput( float *x, float *y )
{
	C_SDKPlayer *pPlayer = C_SDKPlayer::GetLocalSDKPlayer();
	if (!pPlayer)
		return;

	float flSlowMultiplier = RemapValClamped(pPlayer->GetSlowMoMultiplier(), 0.4f, 1, m_slowmodamping.GetFloat(), 1);

	*x *= flSlowMultiplier;
	*y *= flSlowMultiplier;

	*y *= m_verticaldamping.GetFloat();

	C_WeaponSDKBase* pWeapon = pPlayer->GetActiveSDKWeapon();

	if (pWeapon && !pWeapon->HasAimInFireRateBonus())
	{
		float flAimInMultiplier = RemapValClamped(pPlayer->m_Shared.GetAimIn(), 0, 1, 1, m_aimindamping.GetFloat());

		*x *= flAimInMultiplier;
		*y *= flAimInMultiplier;
	}

	BaseClass::OverrideMouseInput(x, y);
}

int ClientModeSDKNormal::HandleSpectatorKeyInput( int down, ButtonCode_t keynum, const char *pszCurrentBinding )
{
	C_SDKPlayer *pPlayer = C_SDKPlayer::GetLocalSDKPlayer();
	if (pPlayer && pPlayer->GetTeamNumber() != TEAM_SPECTATOR)
	{
		// Override base class for +attack, it respawns us.
		if ( down && pszCurrentBinding && Q_strcmp( pszCurrentBinding, "+attack" ) == 0 )
			return 1;
	}

	// Default binding is +alt1 on the right mouse. Have this switch players.
	if ( down && pszCurrentBinding && Q_strcmp( pszCurrentBinding, "+alt1" ) == 0 )
	{
		if (pPlayer && pPlayer->GetTeamNumber() != TEAM_SPECTATOR)
			engine->ClientCmd( "spec_prev" );
		else
			engine->ClientCmd( "spec_next" );

		return 0;
	}

	return BaseClass::HandleSpectatorKeyInput(down, keynum, pszCurrentBinding);
}
