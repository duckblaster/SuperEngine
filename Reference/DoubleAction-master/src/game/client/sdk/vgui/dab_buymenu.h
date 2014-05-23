//======== Copyright � 1996-2008, Valve Corporation, All rights reserved. =========//
//
// Purpose: 
//
// $NoKeywords: $
//=================================================================================//

#ifndef SDK_BUYMENU_H
#define SDK_BUYMENU_H

#include <classmenu.h>
#include <vgui_controls/EditablePanel.h>
#include <FileSystem.h>
#include "iconpanel.h"
#include <vgui_controls/CheckButton.h>

#include "folder_gui.h"

class CWeaponButton : public vgui::Button
{
private:
	DECLARE_CLASS_SIMPLE( CWeaponButton, vgui::Button );
	
public:
	CWeaponButton(vgui::Panel *parent, const char *panelName);

	virtual void ApplySettings( KeyValues *resourceData );
	virtual void ApplySchemeSettings( vgui::IScheme *pScheme );

	virtual void OnCursorEntered();
	virtual void OnCursorExited();

	SDKWeaponID GetWeaponID();

private:
	char m_szWeaponID[100];
	char m_szInfoString[100];
	char m_szInfoModel[100];

	IBorder* m_pArmedBorder;
};

class CDABBuyMenu : public CFolderMenu, public IViewPortPanel
{
private:
	DECLARE_CLASS_SIMPLE( CDABBuyMenu, CFolderMenu );

public:
	CDABBuyMenu(IViewPort *pViewPort);
	virtual ~CDABBuyMenu();

	virtual const char *GetName( void ) { return PANEL_BUY; }

	virtual void Reset();
	virtual void Update( void );
	virtual Panel *CreateControlByName( const char *controlName );
	virtual void OnKeyCodePressed(KeyCode code);
	virtual void SetVisible( bool state );
	virtual void ShowPanel(bool bShow);

	virtual void SetData(KeyValues *data) {};
	virtual bool NeedsUpdate( void ) { return false; }
	virtual bool HasInputElements( void ) { return true; }
	vgui::VPANEL GetVPanel( void ) { return BaseClass::GetVPanel(); }
	virtual bool IsVisible() { return BaseClass::IsVisible(); }
	virtual void SetParent( vgui::VPANEL parent ) { BaseClass::SetParent( parent ); }

	vgui::Label*       GetWeaponInfo();
	class CModelPanel* GetWeaponImage();

protected:
	// vgui overrides for rounded corner background
	virtual void PaintBackground();
	virtual void PaintBorder();
	virtual void ApplySchemeSettings( vgui::IScheme *pScheme );

private:
	IViewPort	*m_pViewPort;

	class CFolderLabel* m_pWeaponInfo;
	class CModelPanel*  m_pWeaponImage;

	CUtlVector<CFolderLabel*> m_apTypes;
	CUtlVector<CFolderLabel*> m_apAmmos;
	CUtlVector<CFolderLabel*> m_apWeights;
	CUtlVector<CFolderLabel*> m_apQuantities;

	ButtonCode_t m_iBuyMenuKey;
};

#endif //SDK_BUYMENU_H