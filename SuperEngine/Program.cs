using OOGL;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Threading;

namespace SuperEngine {
    class SuperEngine : GameWindow {

        Thread gameWindowThread;

        public Thread GameWindowThread {
            get { return gameWindowThread; }
        }


        private static SuperEngine self;

        public static SuperEngine Instance {
            get {
                return self;
            }
        }

        public SuperEngine() {
            self = this;
            gameWindowThread = Thread.CurrentThread;

            this.Width = 800;
            this.Height = 600;
            this.Title = "OOGL Example";

            this.VSync = VSyncMode.Off;

            this.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Game_KeyDown);
            this.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Game_KeyUp);

            this.Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Game_MouseDown);
            this.Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Game_MouseUp);
            this.Mouse.Move += new EventHandler<MouseMoveEventArgs>(Game_MouseMove);
            this.Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(Game_WheelChanged);

        }


        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Multisample);

            SetupLighting();
        }

        protected override void OnResize(EventArgs e) {
            Console.WriteLine(string.Format("Resizing ViewPort: width={0},height={1}", this.Width, this.Height));

            GL.Viewport(0, 0, this.Width, this.Height);

            float aspectRatio = this.Width / (float)this.Height;
            float fieldOfView = (float)Math.PI / 4.0f;

            this.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 1.0f, 1000.0f);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(Matrix4Helper.ToOpenGL(this.projectionMatrix));

            base.OnResize(e);
        }

        private void Game_MouseDown(object sender, MouseButtonEventArgs e) {
        }

        private void Game_MouseUp(object sender, MouseButtonEventArgs e) {
        }

        private void Game_MouseMove(object sender, MouseMoveEventArgs e) {
            if (Mouse[MouseButton.Right]) {
                if (e.XDelta != 0) cameraYaw += OpenTK.MathHelper.DegreesToRadians(e.XDelta * 0.5f);
                if (e.YDelta != 0) cameraPitch += OpenTK.MathHelper.DegreesToRadians(e.YDelta * 0.5f);
            } else {
                if (e.XDelta != 0 || e.YDelta != 0) {
                }
            }
        }

        private void Game_WheelChanged(object sender, MouseWheelEventArgs e) {
            chaseDistance -= e.Delta;
        }

        private void Game_KeyDown(object sender, KeyboardKeyEventArgs e) {

            if (Keyboard[Key.Escape]) Exit();
            if (Keyboard[Key.F1]) SetPolygonMode(PolygonMode.Fill);
            if (Keyboard[Key.F2]) SetPolygonMode(PolygonMode.Line);

            if (Keyboard[Key.Home]) chaseDistance -= 1;
            if (Keyboard[Key.End]) chaseDistance += 1;
        }

        private void Game_KeyUp(object sender, KeyboardKeyEventArgs e) {
        }

        private Matrix4 projectionMatrix;
        public Matrix4 ProjectionMatrix {
            get {
                return projectionMatrix;
            }
        }

        private void SetupLighting() {
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);

            GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 0.5f, 0.5f, 0.5f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 100.0f, 10.0f, 1000.0f, 1.0f });
        }

        private PolygonMode polygonMode = PolygonMode.Fill;
        public void SetPolygonMode(PolygonMode polygonMode) {
            this.polygonMode = polygonMode;
        }

        private float chaseDistance = 10;
        private float cameraYaw = 0;
        private float cameraPitch = 0;

        private float actualFps;
        public float ActualFps {
            get {
                return actualFps;
            }
        }

        private DateTime start = DateTime.Now;
        private long frames = 0;
        private void UpdateActualFps() {
            frames++;

            if (DateTime.Now < start.AddSeconds(1)) return;

            actualFps = (float)(frames / (DateTime.Now - start).TotalSeconds);
            frames = 0;
            start = DateTime.Now;
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            UpdateActualFps();
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.ClearColor(Color.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(OOGL.Matrix4Helper.ToOpenGL(ProjectionMatrix));

            Matrix4 viewMatrix = Matrix4.LookAt(new Vector3(0, 0, chaseDistance), Vector3.Zero, Vector3.UnitY);

            GL.PolygonMode(MaterialFace.FrontAndBack, this.polygonMode);
            ErrorChecking.TraceErrors();

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            SwapBuffers();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            gui.Close();
            base.OnClosing(e);
        }

        public override void Exit() {
            base.Exit();
        }

        protected override void Dispose(bool manual) {
            base.Dispose(manual);
        }

        static DebugGUI gui;

        static void Main() {
            SuperEngine engine = new SuperEngine();
            gui = new DebugGUI();
            Thread guiThread = new Thread(() => { System.Windows.Forms.Application.Run(gui); });
            guiThread.Start();
            engine.Run();
        }
    }
}