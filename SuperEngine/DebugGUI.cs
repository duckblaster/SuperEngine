﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperEngine {
    public partial class DebugGUI : Form {
        public DebugGUI() {
            InitializeComponent();

            propertyGrid1.SelectedObject = SuperEngine.Instance;
        }

        public new void Close() {
            this.UIThread(base.Close);
        }

        protected override void OnClosed(EventArgs e) {
            SuperEngine.Instance.Exit();
            base.OnClosed(e);
        }

        public object SelectedObject
        {
            get
            {
                return propertyGrid1.SelectedObject;
            }
            set
            {
                propertyGrid1.SelectedObject = value;
            }
        }

        private void DebugGUI_Shown(object sender, EventArgs e)
        {
            //SelectedObject = SuperEngine.Instance;
        }
    }

    static class ControlExtensions {
        static public void UIThread(this Control control, Action code) {
            if (control.InvokeRequired) {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        static public void UIThreadInvoke(this Control control, Action code) {
            if (control.InvokeRequired) {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }
}
