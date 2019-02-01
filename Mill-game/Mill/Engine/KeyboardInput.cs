using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mill.Engine {
    public class KeyboardInput {

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        private Control _openTKControl;
        public KeyPressEventHandler KeyPressEvent;

        class KeyState {

            private bool _keyPressDetected = false;
            public bool Held { get; set; }
            public bool Pressed { get; set; }

            public KeyState() {

                Held = false;
                Pressed = false;
            }

            internal void OnDown() {

                if (Held == false) {
                    _keyPressDetected = true;
                }
                Held = true;
            }

            internal void OnUp() {

                Held = false;
            }

            internal void Process() {

                Pressed = false;
                if (_keyPressDetected) {
                    Pressed = true;
                    _keyPressDetected = false;
                }
            }
        }

        private Dictionary<Keys, KeyState> _keyStates = new Dictionary<Keys, KeyState>();

        public KeyboardInput(Control openTKControl) {

            _openTKControl = openTKControl;
            _openTKControl.KeyDown += new KeyEventHandler(OnKeyDown);
            _openTKControl.KeyUp += new KeyEventHandler(OnKeyUp);
            _openTKControl.KeyPress += new KeyPressEventHandler(OnKeyPress);
        }

        
        private void OnKeyDown(object sender, KeyEventArgs e) {

            EnsureKeyStateExits(e.KeyCode);
            _keyStates[e.KeyCode].OnDown();
        }

        private void OnKeyUp(object sender, KeyEventArgs e) {

            EnsureKeyStateExits(e.KeyCode);
            _keyStates[e.KeyCode].OnUp();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e) {

            if (KeyPressEvent != null) {
                KeyPressEvent(sender, e);
            }
        }

        public bool IsKeyPressed(Keys key) {

            EnsureKeyStateExits(key);
            return _keyStates[key].Pressed;
        }

        private void EnsureKeyStateExits(Keys key) {

            if (!_keyStates.Keys.Contains(key)) {
                _keyStates.Add(key, new KeyState());
            }
        }

        public bool IsKeyHeld(Keys key) {

            EnsureKeyStateExits(key);
            return _keyStates[key].Held;
        }

        public void Process() {

            ProcessControlKeys();
            foreach (KeyState state in _keyStates.Values) {
                state.Pressed = false;
                state.Process();
            }
        }

        private bool PollKeyPress(Keys key) {

            return (GetAsyncKeyState((int)key) != 0);
        }

        private void ProcessControlKeys() {

            UpdateControlKey(Keys.Left);
            UpdateControlKey(Keys.Right);
            UpdateControlKey(Keys.Up);
            UpdateControlKey(Keys.Down);
            UpdateControlKey(Keys.LMenu);
        }

        private void UpdateControlKey(Keys keys) {

            if (PollKeyPress(keys)) {
                OnKeyDown(this, new KeyEventArgs(keys));
            } else {
                OnKeyUp(this, new KeyEventArgs(keys));
            }
        }


    }
}
