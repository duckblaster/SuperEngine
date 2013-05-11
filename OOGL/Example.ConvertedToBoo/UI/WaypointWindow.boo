// Copyright (C) 2008 James P Michels III
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//



namespace Example

import System
import System.Collections.Generic
import System.Text
import System.Drawing
import OOGL.GUI
import OOGL.GUI.Abstract
import OOGL.GUI.Widgets

internal class WaypointWindow(BaseFrame):

	public btnAdd as Button

	public btnDelete as Button

	public lstWaypoint as ListBox

	public txtStatus as Textbox

	
	public def constructor(frameMgr as BaseFrameMgr):
		super(frameMgr, 2, 'Waypoint')
		Text = 'Waypoints'
		Location = Point(75, 75)
		Size = Size(100, 50)
		BorderSize = 1
		
		lstWaypoint.Items = (of string: 'ListItem0', 'ListItem1', 'ListItem2', 'ListItem3', 'ListItem4', 'ListItem5', 'ListItem6', 'ListItem7', 'ListItem8')

	
	public override def InitializeControls():
		super.InitializeControls()
		
		self.btnAdd = Button(frameMgr, self)
		self.btnAdd.Text = 'Add'
		self.btnAdd.BorderSize = 1
		self.btnAdd.Size = Size(10, 20)
		AddChildControl(self.btnAdd)
		
		self.btnDelete = Button(frameMgr, self)
		self.btnDelete.Text = 'Delete'
		self.btnDelete.BorderSize = 1
		self.btnDelete.Size = Size(10, 20)
		AddChildControl(self.btnDelete)
		
		self.lstWaypoint = ListBox(frameMgr, self)
		self.lstWaypoint.BorderSize = 0
		AddChildControl(self.lstWaypoint)
		
		self.txtStatus = Textbox(frameMgr, self)
		self.txtStatus.BorderSize = 1
		AddChildControl(self.txtStatus)

	
	public override def OnResize():
		super.OnResize()
		
		btnWidth as int = ((clientRectangle.Width - 3) / 2)
		btnHeight as int = btnAdd.Height
		
		resizeHeight as int = (clientRectangle.Height - (btnHeight + 2))
		waypointHeight as int = (resizeHeight / 2)
		statusHeight as int = (resizeHeight - waypointHeight)
		
		btnAdd.Location = Point(1, 1)
		btnAdd.Size = Size(btnWidth, btnHeight)
		
		btnDelete.Location = Point(((1 + btnWidth) + 1), 1)
		btnDelete.Size = Size(btnWidth, btnHeight)
		
		lstWaypoint.Location = Point(1, ((1 + btnHeight) + 1))
		lstWaypoint.Size = Size((clientRectangle.Width - 2), waypointHeight)
		
		txtStatus.Location = Point(1, ((((1 + btnHeight) + 1) + waypointHeight) + 1))
		txtStatus.Size = Size((clientRectangle.Width - 2), statusHeight)

	
	private nextUpdate as DateTime = DateTime.Now

	
	public override def Update(gameTime as double):
		if DateTime.Now > nextUpdate:
			nextUpdate = DateTime.Now.AddSeconds(5)
			NeedUpdate()
			
			lstWaypoint.ClearItems()
			
			now as DateTime = DateTime.Now
			txtStatus.Text = string.Format('Hour = {0}\nMinute = {1}\nSecond = {2}', now.Hour, now.Minute, now.Second)
		
		super.Update(gameTime)

