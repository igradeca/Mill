using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mill.Engine {
    class MouseInput {

        private Form _parentForm;
        private Control _openTKControl;

        public Engine.Point Position { get; set; }

        private bool _leftClickDetect = false;
        private bool _rightClickDetect = false;
        private bool _middleClickDetect = false;

        public bool LeftPressed { get; private set; }
        public bool RightPressed { get; private set; }
        public bool MiddlePressed { get; private set; }

        public bool LeftHeld { get; private set; }
        public bool RightHeld { get; private set; }
        public bool MiddleHeld { get; private set; }

        public MouseInput(Form form, Control openTKControl) {

            _parentForm = form;
            _openTKControl = openTKControl;

            _openTKControl.MouseClick += delegate (object obj, MouseEventArgs e) {

                if (e.Button == MouseButtons.Left) {
                    _leftClickDetect = true;
                } else if (e.Button == MouseButtons.Right) {
                    _rightClickDetect = true;
                } else if (e.Button == MouseButtons.Middle) {
                    _middleClickDetect = true;
                }
            };

            _openTKControl.MouseDown += delegate (object obj, MouseEventArgs e) {

                if (e.Button == MouseButtons.Left) {
                    LeftHeld = true;
                } else if (e.Button == MouseButtons.Right) {
                    RightHeld = true;
                } else if (e.Button == MouseButtons.Middle) {
                    MiddleHeld = true;
                }
            };

            _openTKControl.MouseUp += delegate (object obj, MouseEventArgs e) {

                if (e.Button == MouseButtons.Left) {
                    LeftHeld = false;
                } else if (e.Button == MouseButtons.Right) {
                    RightHeld = false;
                } else if (e.Button == MouseButtons.Middle) {
                    MiddleHeld = false;
                }
            };

            _openTKControl.MouseLeave += delegate (object obj, EventArgs e) {
                // If we leave control panel
                LeftHeld = false;
                RightHeld = false;
                MiddleHeld = false;
            };
        }

        public void Update(double elapsedTime) {

            UpdateMousePosition();
            UpdateMouseButtons();
        }

        private void UpdateMousePosition() {

            System.Drawing.Point mousePosition = Cursor.Position;
            mousePosition = _openTKControl.PointToClient(mousePosition);

            Engine.Point adjustedMousePoint = new Engine.Point();
            adjustedMousePoint.X = (float)mousePosition.X - ((float)_openTKControl.ClientSize.Width / 2);
            adjustedMousePoint.Y = ((float)_openTKControl.ClientSize.Height / 2) - (float)mousePosition.Y;

            Position = adjustedMousePoint;
        }

        private void UpdateMouseButtons() {

            LeftPressed = false;
            RightPressed = false;
            MiddlePressed = false;

            if (_leftClickDetect) {
                LeftPressed = true;
                _leftClickDetect = false;
            }

            if (_rightClickDetect) {
                RightPressed = true;
                _rightClickDetect = false;
            }

            if (_middleClickDetect) {
                MiddlePressed = true;
                _middleClickDetect = false;
            }
        }


    }
}
