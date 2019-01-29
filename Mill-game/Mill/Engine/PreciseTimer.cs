using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class PreciseTimer {

        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceCounter(ref long PerformanceCounter);

        long _ticksPerSecond = 0;
        long _previousElapsedTime = 0;

        public PreciseTimer() {
            QueryPerformanceFrequency(ref _ticksPerSecond);
            GetElapsedTime();
        }

        public double GetElapsedTime() {

            long time = 0;
            QueryPerformanceCounter(ref time);
            double elapsedTime = (double)(time - _previousElapsedTime) / (double)_ticksPerSecond;
            _previousElapsedTime = time;

            return elapsedTime;
        }

    }
}
