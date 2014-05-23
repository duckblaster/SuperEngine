//======== Copyright � 1996-2008, Valve Corporation, All rights reserved. =========//
//
// Purpose: 
//
// $NoKeywords: $
//=================================================================================//

#include "cbase.h"
#include <stdio.h>
#include <string>
#include <sstream>

#include <cdll_client_int.h>

#include <vgui/IScheme.h>
#include <vgui/ILocalize.h>
#include <vgui/ISurface.h>
#include <KeyValues.h>
#include <vgui_controls/ImageList.h>
#include <FileSystem.h>

#include <vgui_controls/TextEntry.h>
#include <vgui_controls/Button.h>
#include <vgui_controls/RichText.h>
#include <vgui/IVGUI.h>

#include <vgui_controls/Panel.h>

#include "cdll_util.h"

#include <game/client/iviewport.h>
#include "IGameUIFuncs.h" // for key bindings

#include "basemodelpanel.h"

#include "ammodef.h"

#include "sdk_backgroundpanel.h"

#include "sdk_gamerules.h"
#include "c_sdk_player.h"
#include "c_sdk_team.h"

#include "dab_buymenu.h"
#include "folder_gui.h"
#include "da.h"

// memdbgon must be the last include file in a .cpp file!!!
#include "tier0/memdbgon.h"

using namespace vgui;

ConVar _cl_buymenuopen( "_cl_buymenuopen", "0", FCVAR_CLIENTCMD_CAN_EXECUTE, "internal cvar used to tell server when buy menu is open" );

CWeaponButton::CWeaponButton(vgui::Panel *parent, const char *panelName )
	: Button( parent, panelName, "WeaponButton")
{
	m_pArmedBorder = nullptr;

	SetScheme(vgui::scheme()->LoadSchemeFromFile("resource/FolderScheme.res", "FolderScheme"));
	InvalidateLayout(true, true);
}

void CWeaponButton::ApplySettings( KeyValues *resourceData )
{
	BaseClass::ApplySettings( resourceData );

	strcpy(m_szInfoString, resourceData->GetString("info_string"));
	strcpy(m_szInfoModel, resourceData->GetString("info_model"));
	strcpy(m_szWeaponID, resourceData->GetString("weaponid"));
}

void CWeaponButton::ApplySchemeSettings( vgui::IScheme *pScheme )
{
	BaseClass::ApplySchemeSettings( pScheme );

	m_pArmedBorder = pScheme->GetBorder("FolderButtonArmedBorder");
}

void CWeaponButton::OnCursorEntered()
{
	BaseClass::OnCursorEntered();

	InvalidateLayout();
	SetBorder(m_pArmedBorder);

	CDABBuyMenu* pParent = dynamic_cast<CDABBuyMenu*>(GetParent());
	if (!pParent)
		return;

	vgui::Label* pInfoLabel = pParent->GetWeaponInfo();
	if (pInfoLabel)
	{
		if (m_szWeaponID[0])
			pInfoLabel->SetText((std::string("#weaponinfo_") + m_szWeaponID).c_str());
		else
			pInfoLabel->SetText(m_szInfoString);
	}

	CModelPanel* pInfoModel = pParent->GetWeaponImage();
	if (pInfoModel)
	{
		if (!strlen(m_szInfoModel))
			pInfoModel->SwapModel("");
		else
			pInfoModel->SwapModel(m_szInfoModel);
	}
}

void CWeaponButton::OnCursorExited()
{
	BaseClass::OnCursorExited();

	InvalidateLayout();
}

SDKWeaponID CWeaponButton::GetWeaponID()
{
	return AliasToWeaponID(m_szWeaponID);
}

CDABBuyMenu::CDABBuyMenu(IViewPort* pViewPort) : CFolderMenu( PANEL_BUY )
{
	m_pViewPort = pViewPort;

	m_iBuyMenuKey = BUTTON_CODE_INVALID;

	LoadControlSettings( "Resource/UI/BuyMenu.res" );
	InvalidateLayout();

	m_pWeaponInfo = dynamic_cast<CFolderLabel*>(FindChildByName("WeaponInfo"));
	m_pWeaponImage = dynamic_cast<CModelPanel*>(FindChildByName("WeaponImage"));
}

//Destructor
CDABBuyMenu::~CDABBuyMenu()
{
}

void CDABBuyMenu::Reset()
{
	m_pWeaponInfo->SetText("");
	m_pWeaponImage->SwapModel("");
}

void CDABBuyMenu::ShowPanel( bool bShow )
{
	if ( bShow )
		m_iBuyMenuKey = gameuifuncs->GetButtonCodeForBind( "buy" );

	m_pWeaponInfo->SetText("");
	m_pWeaponImage->SwapModel("");

	if ( bShow )
	{
		Activate();
		SetMouseInputEnabled( true );
	}
	else
	{
		SetVisible( false );
		SetMouseInputEnabled( false );
	}
}

void CDABBuyMenu::OnKeyCodePressed( KeyCode code )
{
	if ( code == KEY_PAD_ENTER || code == KEY_ENTER )
	{
		engine->ClientCmd("buy random");
		OnCommand("close");
	}
	else if ( m_iBuyMenuKey != BUTTON_CODE_INVALID && m_iBuyMenuKey == code )
	{
		ShowPanel( false );
	}
	else
	{
		BaseClass::OnKeyCodePressed( code );
	}
}

static ConVar hud_playerpreview_x("hud_playerpreview_x", "120", FCVAR_CHEAT|FCVAR_DEVELOPMENTONLY);
static ConVar hud_playerpreview_y("hud_playerpreview_y", "-5", FCVAR_CHEAT|FCVAR_DEVELOPMENTONLY);
static ConVar hud_playerpreview_z("hud_playerpreview_z", "-57", FCVAR_CHEAT|FCVAR_DEVELOPMENTONLY);

void CDABBuyMenu::Update()
{
	m_pWeaponInfo = dynamic_cast<CFolderLabel*>(FindChildByName("WeaponInfo"));
	m_pWeaponInfo->SetText("");

	m_pWeaponImage = dynamic_cast<CModelPanel*>(FindChildByName("WeaponImage"));
	m_pWeaponImage->SwapModel("");

	Button *entry = dynamic_cast<Button *>(FindChildByName("CancelButton"));
	if (entry)
		entry->SetVisible(true);

	CFolderLabel* pWeaponType = dynamic_cast<CFolderLabel*>(FindChildByName("WeaponType"));
	int iWeaponTypeX, iWeaponTypeY;
	pWeaponType->GetPos(iWeaponTypeX, iWeaponTypeY);

	CFolderLabel* pWeaponAmmo = dynamic_cast<CFolderLabel*>(FindChildByName("WeaponAmmo"));
	int iWeaponAmmoX, iWeaponAmmoY;
	pWeaponAmmo->GetPos(iWeaponAmmoX, iWeaponAmmoY);

	CFolderLabel* pWeaponWeight = dynamic_cast<CFolderLabel*>(FindChildByName("WeaponWeight"));
	int iWeaponWeightX, iWeaponWeightY;
	pWeaponWeight->GetPos(iWeaponWeightX, iWeaponWeightY);

	CFolderLabel* pWeaponQuantity = dynamic_cast<CFolderLabel*>(FindChildByName("WeaponQuantity"));
	int iWeaponQuantityX, iWeaponQuantityY;
	pWeaponQuantity->GetPos(iWeaponQuantityX, iWeaponQuantityY);

	C_SDKPlayer *pPlayer = C_SDKPlayer::GetLocalSDKPlayer();

	if (!pPlayer)
		return;

	Label *pSlotsLabel = dynamic_cast<Label *>(FindChildByName("SlotsRemaining"));
	if (pSlotsLabel)
	{
		wchar_t szFmt[128]=L"";
		const wchar_t *pchFmt = g_pVGuiLocalize->Find( "#DAB_BuyMenu_SlotsRemaining" );
		if ( pchFmt && pchFmt[0] )
		{
			wchar_t szText[512]=L"";
			wchar_t szLoadoutWeight[ 10 ];

			Q_wcsncpy( szFmt, pchFmt, sizeof( szFmt ) );
			_snwprintf( szLoadoutWeight, ARRAYSIZE(szLoadoutWeight) - 1, L"%d",  MAX_LOADOUT_WEIGHT-pPlayer->GetLoadoutWeight() );
			g_pVGuiLocalize->ConstructString( szText, sizeof( szText ), szFmt, 1, szLoadoutWeight );

			pSlotsLabel->SetText(szText);
		}
	}

	for ( int i = 0; i < m_apTypes.Count(); i++)
	{
		m_apTypes[i]->DeletePanel();
		m_apTypes[i] = nullptr;
	}

	for ( int i = 0; i < m_apAmmos.Count(); i++)
	{
		m_apAmmos[i]->DeletePanel();
		m_apAmmos[i] = nullptr;
	}

	for ( int i = 0; i < m_apWeights.Count(); i++)
	{
		m_apWeights[i]->DeletePanel();
		m_apWeights[i] = nullptr;
	}

	for ( int i = 0; i < m_apQuantities.Count(); i++)
	{
		m_apQuantities[i]->DeletePanel();
		m_apQuantities[i] = nullptr;
	}

	m_apTypes.RemoveAll();
	m_apAmmos.RemoveAll();
	m_apWeights.RemoveAll();
	m_apQuantities.RemoveAll();

	CUtlVector<CWeaponButton*> apWeaponButtons;

	for ( int i = 0 ; i < GetChildCount() ; ++i )
	{
		// Hide the subpanel for the CWeaponButtons
		CWeaponButton *pPanel = dynamic_cast<CWeaponButton *>( GetChild( i ) );

		if (!pPanel)
			continue;

		if (pPanel->GetWeaponID() != WEAPON_NONE)
			apWeaponButtons.AddToTail(pPanel);

		pPanel->SetEnabled(true);

		if (!pPanel->GetName())
			continue;

		if (strlen(pPanel->GetName()) < 7)
			continue;

		SDKWeaponID eWeapon = AliasToWeaponID(pPanel->GetName() + 7);

		if (!eWeapon)
			continue;

		pPanel->SetEnabled(pPlayer->CanAddToLoadout(eWeapon));
		pPanel->InvalidateLayout(true);
	}

	for (int i = 0; i < apWeaponButtons.Count(); i++)
	{
		CWeaponButton* pPanel = apWeaponButtons[i];

		int iWeaponX, iWeaponY;
		pPanel->GetPos(iWeaponX, iWeaponY);

		CSDKWeaponInfo* pInfo = CSDKWeaponInfo::GetWeaponInfo(pPanel->GetWeaponID());

		m_apTypes.AddToTail(new CFolderLabel(this, nullptr));

		m_apTypes.Tail()->SetText((std::string("#DA_WeaponType_") + WeaponTypeToAlias(pInfo->m_eWeaponType)).c_str());
		m_apTypes.Tail()->SetPos(iWeaponTypeX, iWeaponY);
		m_apTypes.Tail()->SetZPos(-5);
		m_apTypes.Tail()->SetFont(vgui::scheme()->GetIScheme(GetScheme())->GetFont("FolderSmall"));
		m_apTypes.Tail()->SetScheme("FolderScheme");

		m_apAmmos.AddToTail(new CFolderLabel(this, nullptr));

		m_apAmmos.Tail()->SetText((std::string("#DA_Ammo_") + pInfo->szAmmo1).c_str());
		m_apAmmos.Tail()->SetPos(iWeaponAmmoX, iWeaponY);
		m_apAmmos.Tail()->SetZPos(-5);
		m_apAmmos.Tail()->SetFont(vgui::scheme()->GetIScheme(GetScheme())->GetFont("FolderSmall"));
		m_apAmmos.Tail()->SetScheme("FolderScheme");

		m_apWeights.AddToTail(new CFolderLabel(this, nullptr));

		std::ostringstream sWeight;
		sWeight << pInfo->iWeight;
		m_apWeights.Tail()->SetText(sWeight.str().c_str());
		m_apWeights.Tail()->SetPos(iWeaponWeightX, iWeaponY);
		m_apWeights.Tail()->SetZPos(-5);
		m_apWeights.Tail()->SetFont(vgui::scheme()->GetIScheme(GetScheme())->GetFont("FolderMedium"));
		m_apWeights.Tail()->SetScheme("FolderScheme");

		if (pPlayer->GetLoadoutWeaponCount(pPanel->GetWeaponID()))
		{
			m_apQuantities.AddToTail(new CFolderLabel(this, nullptr));

			std::ostringstream sCount;
			sCount << pPlayer->GetLoadoutWeaponCount(pPanel->GetWeaponID());
			m_apQuantities.Tail()->SetText(sCount.str().c_str());
			m_apQuantities.Tail()->SetPos(iWeaponQuantityX, iWeaponY);
			m_apQuantities.Tail()->SetZPos(-5);
			m_apQuantities.Tail()->SetFont(vgui::scheme()->GetIScheme(GetScheme())->GetFont("FolderMedium"));
			m_apQuantities.Tail()->SetScheme("FolderScheme");
		}
	}

	BaseClass::Update();
}

Panel *CDABBuyMenu::CreateControlByName( const char *controlName )
{
	if (FStrEq(controlName, "WeaponButton"))
		return new CWeaponButton(this, nullptr);

	return BaseClass::CreateControlByName(controlName);
}

void CDABBuyMenu::SetVisible( bool state )
{
	BaseClass::SetVisible( state );

	if ( state )
	{
		engine->ServerCmd( "menuopen" );			// to the server
		engine->ClientCmd( "_cl_buymenuopen 1" );	// for other panels
	}
	else
	{
		engine->ServerCmd( "menuclosed" );	
		engine->ClientCmd( "_cl_buymenuopen 0" );
	}
}

void CDABBuyMenu::PaintBackground()
{
	// Don't
}

void CDABBuyMenu::PaintBorder()
{
	// Don't
}

//-----------------------------------------------------------------------------
// Purpose: Apply scheme settings
//-----------------------------------------------------------------------------
void CDABBuyMenu::ApplySchemeSettings( vgui::IScheme *pScheme )
{
	BaseClass::ApplySchemeSettings( pScheme );

	DisableFadeEffect(); //Tony; shut off the fade effect because we're using sourcesceheme.
}

vgui::Label* CDABBuyMenu::GetWeaponInfo()
{
	return m_pWeaponInfo;
}

CModelPanel* CDABBuyMenu::GetWeaponImage()
{
	return m_pWeaponImage;
}

CON_COMMAND(hud_reload_buy, "Reload resource for buy menu.")
{
	IViewPortPanel *pPanel = gViewPortInterface->FindPanelByName( PANEL_BUY );
	CDABBuyMenu *pBuy = dynamic_cast<CDABBuyMenu*>(pPanel);
	if (!pBuy)
		return;

	pBuy->LoadControlSettings( "Resource/UI/BuyMenu.res" );
	pBuy->InvalidateLayout();
	pBuy->Update();
}
