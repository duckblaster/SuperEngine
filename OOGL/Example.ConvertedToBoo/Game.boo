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
import System.Drawing
import System.Diagnostics
import System.Threading
import System.IO
import OpenTK
import OpenTK.Graphics
import OpenTK.Input
import OpenTK.Platform
import OOGL.Animation
import OOGL.Shaders
import OOGL.Textures
import OOGL
import OOGL.GUI
import OOGL.GUI.Abstract
import OOGL.GUI.VertexStructures

internal class Game(GameWindow):

	private frameMgr as BaseFrameMgr

	private model as Model

	private controller as Controller

	
	private static game as Game

	public static Instance as Game:
		get:
			return game

	
	public def constructor():
		game = self
		
		self.Width = 800
		self.Height = 600
		self.Title = 'OOGL Example'
		
		self.VSync = VSyncMode.Off
		
		ResourceLocator.RootPath = (System.Environment.CurrentDirectory + '/../../../Media/')
		
		positionColorShader = ShaderProgram(ResourceLocator.GetFullPath('Shaders/guiColored.vs'), ResourceLocator.GetFullPath('Shaders/guiColored.fs'))
		positionTextureShader = ShaderProgram(ResourceLocator.GetFullPath('Shaders/guiTextured.vs'), ResourceLocator.GetFullPath('Shaders/guiTextured.fs'))
		
		self.frameMgr = BaseFrameMgr(self, positionColorShader, positionTextureShader)
		
		self.Keyboard.KeyDown += Game_KeyDown
		self.Keyboard.KeyUp += Game_KeyUp
		
		self.Mouse.ButtonDown += Game_MouseDown
		self.Mouse.ButtonUp += Game_MouseUp
		self.Mouse.Move += Game_MouseMove
		self.Mouse.WheelChanged += Game_WheelChanged
		
		DemoWindow(self.frameMgr)
		PlayerWindow(self.frameMgr)
		RadarWindow(self.frameMgr)
		WaypointWindow(self.frameMgr)
		
		self.frameMgr.LoadWorkspace()
		
		ms3dFile as Ms3dLoader.File = Ms3dLoader.File(ResourceLocator.GetFullPath('Models/Beta_Kamujin/Beta_Kamujin.ms3d'))
		
		tracks as (Sample) = Sample.Load(ResourceLocator.GetFullPath('Models/Beta_Kamujin/Beta_Kamujin.animations'))
		modelShader = ShaderProgram(ResourceLocator.GetFullPath('Shaders/skeletalAnimation.vs'), ResourceLocator.GetFullPath('Shaders/skeletalAnimation.fs'))
		self.model = ms3dFile.ToModel(modelShader, tracks)
		self.controller = Controller(model)

	
	
	protected override def OnLoad(e as EventArgs):
		super.OnLoad(e)
		
		GL.Enable(EnableCap.DepthTest)
		GL.DepthFunc(DepthFunction.Lequal)
		
		GL.CullFace(CullFaceMode.Back)
		GL.Enable(EnableCap.CullFace)
		//			GL.Disable(EnableCap.CullFace);
		GL.Enable(EnableCap.Multisample)
		
		SetupLighting()

	
	protected override def OnResize(e as EventArgs):
		Console.WriteLine(string.Format('Resizing ViewPort: width={0},height={1}', self.Width, self.Height))
		
		GL.Viewport(0, 0, self.Width, self.Height)
		
		aspectRatio as single = (self.Width / (self.Height cast single))
		fieldOfView as single = ((System.Math.PI cast single) / 4.0F)
		
		self.projectionMatrix = Matrix4.Perspective(fieldOfView, aspectRatio, 1.0F, 1000.0F)
		
		GL.MatrixMode(MatrixMode.Projection)
		GL.LoadMatrix(Matrix4Helper.ToOpenGL(self.projectionMatrix))
		
		super.OnResize(e)

	
	private def Game_MouseDown(sender as object, e as MouseButtonEventArgs):
		/* bool consumed = */
		frameMgr.OnMouseDown(sender, e)

	
	private def Game_MouseUp(sender as object, e as MouseButtonEventArgs):
		/* bool consumed = */
		frameMgr.OnMouseUp(sender, e)

	
	private def Game_MouseMove(sender as object, e as MouseMoveEventArgs):
		if Mouse[MouseButton.Right]:
			if e.XDelta != 0:
				cameraYaw += ((e.XDelta * Functions.DTORF) * 0.5F)
			if e.YDelta != 0:
				cameraPitch += ((e.YDelta * Functions.DTORF) * 0.5F)
		elif (e.XDelta != 0) or (e.YDelta != 0):
			/* bool consumed = */
			frameMgr.OnMouseMove(sender, e)

	
	private def Game_WheelChanged(sender as object, e as MouseWheelEventArgs):
		chaseDistance -= e.Delta

	
	//private void Game_KeyDown(KeyboardDevice keyboard, Key key)
	//{
	//    if (frameMgr.ConsumingKeyboard)
	//    {
	//        frameMgr.OnKeyDown(keyboard, key);
	//    }
	
	//    if (Keyboard[Key.Escape]) Exit();
	//    if (Keyboard[Key.F1]) SetPolygonMode(PolygonMode.Fill);
	//    if (Keyboard[Key.F2]) SetPolygonMode(PolygonMode.Line);
	//    if (Keyboard[Key.F3]) this.controller.CrossfadeTo("Battloid-Transport");
	//    if (Keyboard[Key.F4]) this.controller.CrossfadeTo("Transport-Battloid");
	//    if (Keyboard[Key.F5]) this.controller.CrossfadeTo("Run");
	
	//    if (Keyboard[Key.Home]) chaseDistance -= 1;
	//    if (Keyboard[Key.End]) chaseDistance += 1;				
	//}
	
	private def Game_KeyDown(sender as object, e as KeyboardKeyEventArgs):
		if frameMgr.ConsumingKeyboard:
			frameMgr.OnKeyDown(sender, e)
		
		if Keyboard[Key.Escape]:
			Exit()
		if Keyboard[Key.F1]:
			SetPolygonMode(PolygonMode.Fill)
		if Keyboard[Key.F2]:
			SetPolygonMode(PolygonMode.Line)
		if Keyboard[Key.F3]:
			self.controller.CrossfadeTo('Battloid-Transport')
		if Keyboard[Key.F4]:
			self.controller.CrossfadeTo('Transport-Battloid')
		if Keyboard[Key.F5]:
			self.controller.CrossfadeTo('Run')
		
		if Keyboard[Key.Home]:
			chaseDistance -= 1
		if Keyboard[Key.End]:
			chaseDistance += 1

	
	//private void Game_KeyUp(KeyboardDevice keyboard, Key key)
	//{
	//    if (frameMgr.ConsumingKeyboard)
	//    {
	//        frameMgr.OnKeyUp(keyboard, key);
	//    }
	//}
	
	private def Game_KeyUp(sender as object, e as KeyboardKeyEventArgs):
		if frameMgr.ConsumingKeyboard:
			frameMgr.OnKeyUp(sender, e)

	
	private projectionMatrix as Matrix4

	public ProjectionMatrix as Matrix4:
		get:
			return projectionMatrix

	
	private def SetupLighting():
		GL.Enable(EnableCap.Lighting)
		GL.Enable(EnableCap.Light0)
		
		GL.Light(LightName.Light0, LightParameter.Ambient, (of single: 0.0F, 0.0F, 0.0F, 1.0F))
		GL.Light(LightName.Light0, LightParameter.Diffuse, (of single: 1.0F, 1.0F, 1.0F, 1.0F))
		GL.Light(LightName.Light0, LightParameter.Specular, (of single: 0.5F, 0.5F, 0.5F, 1.0F))
		GL.Light(LightName.Light0, LightParameter.Position, (of single: 100.0F, 10.0F, 1000.0F, 1.0F))

	
	private polygonMode as PolygonMode = PolygonMode.Fill

	public def SetPolygonMode(polygonMode as PolygonMode):
		self.polygonMode = polygonMode

	
	private chaseDistance as single = 10

	private cameraYaw as single = 0

	private cameraPitch as single = 0

	
	private actualFps as single

	public ActualFps as single:
		get:
			return actualFps

	
	private start as DateTime = DateTime.Now

	private frames as long = 0

	private def UpdateActualFps():
		frames += 1
		
		if DateTime.Now < start.AddSeconds(1):
			return
		
		actualFps = ((frames / (DateTime.Now - start).TotalSeconds) cast single)
		frames = 0
		start = DateTime.Now

	
	protected override def OnUpdateFrame(e as FrameEventArgs):
		UpdateActualFps()
		
		frameMgr.Update(e.Time)

	
	protected override def OnRenderFrame(e as FrameEventArgs):
		GL.ClearColor(Color.CornflowerBlue)
		GL.Clear((ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit))
		
		GL.MatrixMode(MatrixMode.Projection)
		GL.LoadMatrix(OOGL.Matrix4Helper.ToOpenGL(ProjectionMatrix))
		
		viewMatrix as Matrix4 = Matrix4.LookAt(Vector3(0, 0, chaseDistance), Vector3.Zero, Vector3.UnitY)
		
		GL.PolygonMode(MaterialFace.FrontAndBack, self.polygonMode)
		model.Draw(controller, 0.02500000037F, (Matrix4.Rotate(Vector3.UnitY, Functions.PIF) * viewMatrix))
		ErrorChecking.TraceErrors()
		
		GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill)
		frameMgr.Draw(e.Time)
		
		SwapBuffers()

	
	public override def Exit():
		self.frameMgr.SaveWorkspace()
		
		super.Exit()

	
	protected override def Dispose(manual as bool):
		super.Dispose(manual)

	
	[STAThread]
	internal static def Main():
		listener = TextWriterTraceListener(Console.Out)
		Trace.Listeners.Add(listener)
		
		using game = Game():
			//game.RunSimple(60);
			game.Run(60, 60)

Game.Main()
