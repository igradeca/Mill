using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public static class Utils {

        public static Size gridSize = new Size(10, 10);
        public static float gridPadding = 1f;
        public static float layerOffset = 1.3333333f;

        public enum GameType {

            TwelveMorris,
            NineMorris,
            SixMorris,
            ThreeMoriss,
            LaskerMorris
        }


    }
}
