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
import OpenTK.Graphics
import OOGL
import OOGL.Textures
import OOGL.GUI
import OOGL.GUI.Abstract
import OOGL.GUI.Widgets

internal class RadarWindow(BaseFrame):

	private shortRange as CheckBox

	private mediumRange as CheckBox

	private longRange as CheckBox

	private radar as Picture

	private comboBox as ComboBox

	
	public def constructor(frameMgr as BaseFrameMgr):
		super(frameMgr, 1, 'Radar')
		BackgroundColor = Color.PaleGoldenrod
		BorderColor = Color.PaleGoldenrod
		TitleColor = Color.PaleGoldenrod
		Location = Point(200, 200)
		Size = Size(100, 100)
		Text = 'Radar'

	
	public override def InitializeControls():
		super.InitializeControls()
		
		texture as Texture = TextureManager.Instance.Find(ResourceLocator.GetFullPath('Textures/Radar.dds'), TextureMagFilter.Nearest, TextureMinFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp)
		
		self.radar = Picture(frameMgr, self)
		self.radar.BorderSize = 2
		self.radar.Texture = texture
		AddChildControl(self.radar)
		
		checkBoxWidth = 60
		
		self.shortRange = CheckBox(frameMgr, self)
		self.shortRange.ControlRectangle = Rectangle((0 * (checkBoxWidth + 1)), 1, checkBoxWidth, 20)
		self.shortRange.Text = 'Short'
		self.shortRange.BorderSize = 1
		self.shortRange.Value = true
		self.shortRange.ValueChanged += shortRange_ValueChanged
		AddChildControl(self.shortRange)
		
		self.mediumRange = CheckBox(frameMgr, self)
		self.mediumRange.ControlRectangle = Rectangle((1 * (checkBoxWidth + 1)), 1, checkBoxWidth, 20)
		self.mediumRange.Text = 'Medium'
		self.mediumRange.BorderSize = 1
		self.mediumRange.ValueChanged += mediumRange_ValueChanged
		AddChildControl(self.mediumRange)
		
		self.longRange = CheckBox(frameMgr, self)
		self.longRange.ControlRectangle = Rectangle((2 * (checkBoxWidth + 1)), 1, checkBoxWidth, 20)
		self.longRange.Text = 'Long'
		self.longRange.BorderSize = 1
		self.longRange.ValueChanged += longRange_ValueChanged
		AddChildControl(self.longRange)
		
		self.comboBox = ComboBox(frameMgr, self)
		self.comboBox.ControlRectangle = Rectangle((3 * (checkBoxWidth + 1)), 1, (checkBoxWidth * 2), 20)
		self.comboBox.Text = 'ComboBox'
		self.comboBox.BorderSize = 1
		self.comboBox.Items = (of string: 'Test1', 'Test2', 'Test3')
		self.comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged
		AddChildControl(self.comboBox)
		

	
	private def comboBox_SelectedIndexChanged(sender as object, e as EventArgs):
		selectedIndex as int = comboBox.SelectedIndex
		if selectedIndex == 0:
			Game.Instance.SetPolygonMode(PolygonMode.Fill)
		elif selectedIndex == 1:
			Game.Instance.SetPolygonMode(PolygonMode.Line)
		else:
			Game.Instance.SetPolygonMode(PolygonMode.Point)
		

	
	private def longRange_ValueChanged(sender as object, e as EventArgs):
		if longRange.Value:
			shortRange.Value = false
			mediumRange.Value = false
		Game.Instance.SetPolygonMode(PolygonMode.Point)

	
	private def mediumRange_ValueChanged(sender as object, e as EventArgs):
		if mediumRange.Value:
			shortRange.Value = false
			longRange.Value = false
		Game.Instance.SetPolygonMode(PolygonMode.Line)

	
	private def shortRange_ValueChanged(sender as object, e as EventArgs):
		if shortRange.Value:
			mediumRange.Value = false
			longRange.Value = false
		Game.Instance.SetPolygonMode(PolygonMode.Fill)

	
	public override def OnResize():
		super.OnResize()
		
		//Rectangle screenClientRect = ClientToScreen(clientRectangle);
		self.radar.ControlRectangle = Rectangle(5, (shortRange.Height + 6), (clientRectangle.Width - 10), (clientRectangle.Height - (shortRange.Height + 11)))

