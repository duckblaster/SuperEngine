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
import System.Drawing
import OOGL
import OOGL.Textures
import OOGL.GUI.Abstract
import OOGL.GUI.Widgets
import OpenTK.Graphics

public class DemoWindow(BaseFrame):

	private progressBar as ProgressBar

	private button as Button

	private picture as Picture

	private vScrollBar as VScrollBar

	
	public def constructor(frameMgr as BaseFrameMgr):
		super(frameMgr, 0, 'DemoWindow')
		self.Location = Point(10, 10)
		self.Size = Size(400, 400)

	
	public override def InitializeControls():
		super.InitializeControls()
		
		texture as Texture = TextureManager.Instance.Find(ResourceLocator.GetFullPath('Planets/Earth/earth.dds'), TextureMagFilter.Nearest, TextureMinFilter.Nearest, TextureWrapMode.Clamp, TextureWrapMode.Clamp)
		
		self.progressBar = ProgressBar(self.frameMgr, self)
		self.progressBar.Location = Point(10, 10)
		self.progressBar.Size = Size(200, 20)
		self.progressBar.Value = 75
		self.progressBar.BorderSize = 1
		AddChildControl(self.progressBar)
		
		self.button = Button(self.frameMgr, self)
		self.button.Location = Point(10, 40)
		self.button.Size = Size(100, 25)
		self.button.BorderSize = 1
		AddChildControl(self.button)
		
		self.picture = Picture(self.frameMgr, self)
		self.picture.Location = Point(10, 70)
		self.picture.Size = Size(200, 200)
		self.picture.BorderSize = 1
		self.picture.Texture = texture
		AddChildControl(self.picture)
		
		self.vScrollBar = VScrollBar(self.frameMgr, self)
		self.vScrollBar.Location = Point(220, 10)
		self.vScrollBar.Size = Size(40, 260)
		AddChildControl(self.vScrollBar)

