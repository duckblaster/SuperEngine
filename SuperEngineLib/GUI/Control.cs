using System;
using OpenTK;
using OpenTK.Input;

namespace SuperEngineLib.GUI {
    public class Control {
        #region Positioning

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

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

