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

internal class PlayerWindow(BaseFrame):

	private pgrHealth as ProgressBar

	private pgrMechHead as ProgressBar

	private pgrMechBody as ProgressBar

	private pgrMechLeftArm as ProgressBar

	private pgrMechRightArm as ProgressBar

	private pgrMechLeftLeg as ProgressBar

	private pgrMechRightLeg as ProgressBar

	
	private lblHealth as Label

	private lblMechHead as Label

	private lblMechBody as Label

	private lblMechLeftArm as Label

	private lblMechRightArm as Label

	private lblMechLeftLeg as Label

	private lblMechRightLeg as Label

	
	public def constructor(frameMgr as BaseFrameMgr):
		super(frameMgr, 0, 'Player')
		Location = Point(100, 100)
		Size = Size(100, 100)
		Text = 'Player Name'
		BorderSize = 0
		BackgroundColor = Color.Silver

	
	public override def InitializeControls():
		super.InitializeControls()
		
		InitHelper(lblHealth, pgrHealth, 0, 'Health', 100, Color.Red, Color.LightSalmon, Color.Silver)
		InitHelper(lblMechHead, pgrMechHead, 1, 'Mech Head', 95, Color.SteelBlue, Color.LightSteelBlue, Color.Silver)
		InitHelper(lblMechBody, pgrMechBody, 2, 'Mech Body', 35, Color.SteelBlue, Color.LightSteelBlue, Color.Silver)
		InitHelper(lblMechLeftArm, pgrMechLeftArm, 3, 'Mech Left Arm', 70, Color.SteelBlue, Color.LightSteelBlue, Color.Silver)
		InitHelper(lblMechRightArm, pgrMechRightArm, 4, 'Mech Right Arm', 99, Color.SteelBlue, Color.LightSteelBlue, Color.Silver)
		InitHelper(lblMechLeftLeg, pgrMechLeftLeg, 5, 'Mech Left Leg', 1, Color.SteelBlue, Color.LightSteelBlue, Color.Silver)
		InitHelper(lblMechRightLeg, pgrMechRightLeg, 6, 'Mech Right Leg', 10, Color.SteelBlue, Color.LightSteelBlue, Color.Silver)

	
	public override def OnResize():
		super.OnResize()
		
		rows = 7
		ResizeHelper(lblHealth, pgrHealth, 0, rows)
		ResizeHelper(lblMechHead, pgrMechHead, 1, rows)
		ResizeHelper(lblMechBody, pgrMechBody, 2, rows)
		ResizeHelper(lblMechLeftArm, pgrMechLeftArm, 3, rows)
		ResizeHelper(lblMechRightArm, pgrMechRightArm, 4, rows)
		ResizeHelper(lblMechLeftLeg, pgrMechLeftLeg, 5, rows)
		ResizeHelper(lblMechRightLeg, pgrMechRightLeg, 6, rows)

	
	private def InitHelper(ref label as Label, ref bar as ProgressBar, row as int, text as string, value as int, barColor as Color, barBack as Color, textBack as Color):
		borderSize = 0
		//int spacer = 2;
		//int textWidth = 100;
		//int barLeftEdge = (2 * spacer) + textWidth;
		
		label = Label(frameMgr, self)
		label.Text = text
		label.BorderSize = 0
		label.DrawTextFormat = (DrawTextFormat.Right | DrawTextFormat.VerticalCenter)
		label.BackgroundColor = textBack
		AddChildControl(label)
		
		bar = ProgressBar(frameMgr, self)
		bar.BorderSize = borderSize
		bar.BackgroundColor = barBack
		bar.BarColor = barColor
		bar.Value = value
		AddChildControl(bar)
		
		ResizeHelper(label, bar, row, 7)

	
	private def ResizeHelper(label as Label, bar as ProgressBar, row as int, rows as int):
		clientWidth as int = clientRectangle.Width
		clientHeight as int = clientRectangle.Height
		
		spacer = 2
		spacer2 as int = (spacer * 2)
		textWidth = 100
		barLeftEdge as int = (spacer2 + textWidth)
		
		rowHeight as int = ((clientHeight - (spacer * (rows + 1))) / rows)
		if rowHeight < 5:
			rowHeight = 5
		
		barWidth as int = ((clientWidth - spacer) - barLeftEdge)
		if barWidth < 5:
			barWidth = 5
		
		label.Location = Point(spacer, (spacer + (row * (rowHeight + spacer))))
		label.Size = Size(textWidth, rowHeight)
		
		bar.Location = Point(barLeftEdge, (spacer + (row * (rowHeight + spacer))))
		bar.Size = Size(barWidth, rowHeight)

	
	public override def Update(gameTime as double):
		pgrHealth.Value = (Game.Instance.ActualFps cast int)
		
		super.Update(gameTime)

