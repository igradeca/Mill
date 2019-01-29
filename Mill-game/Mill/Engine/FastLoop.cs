using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mill.Engine {

    [StructLayout(LayoutKind.Sequential)]
    public struct Message {
        public IntPtr hWnd;
        public Int32 msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    public class FastLoop {

        [SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(
            out Message msg,
            IntPtr hWnd,
            uint messageFilterMin,
            uint messageFilterMax,
            uint flags);

        private PreciseTimer _timer = new PreciseTimer();

        public delegate void LoopCallBack(double elapsedTime);
        private LoopCallBack _callback;

        public FastLoop(LoopCallBack callback) {

            _callback = callback;
            Application.Idle += new EventHandler(OnApplicationEnterIdle);
        }

        private void OnApplicationEnterIdle(object sender, EventArgs e) {

            while (IsApplicationStillIdle()) {
                _callback(_timer.GetElapsedTime());
            }
        }

        private bool IsApplicationStillIdle() {

            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }


    }
}
