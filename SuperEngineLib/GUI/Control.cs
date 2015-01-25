using System;
using OpenTK;
using OpenTK.Input;

namespace SuperEngineLib.GUI {
    public class Control {
        #region Positioning

        private int x;
        private int y;
        private int width;
        private int height;

        public int X {
            get {
                return this.x;
            }
            set {
                x = value;
            }
        }

        public int Y {
            get {
                return this.y;
            }
            set {
                y = value;
            }
        }

        public int Width {
            get {
                return this.width;
            }
            set {
                width = value;
            }
        }

        public int Height {
            get {
                return this.height;
            }
            set {
                height = value;
            }
        }

        #endregion

        private bool focused;

        public virtual void OnMouseDown(MouseEventArgs e) {

        }

        public virtual void OnMouseUp(MouseEventArgs e) {

        }

        public virtual void OnKeyDown(KeyboardKeyEventArgs e) {

        }

        public virtual void OnKeyUp(KeyboardKeyEventArgs e) {

        }

        public virtual void Draw() {

        }

        public Control() {
        }
    }
}

