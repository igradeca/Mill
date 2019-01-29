using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public static class Utils {

        public static Point GridCenter = new Point(0f, 0f);
        public static Size GridSize = new Size(10, 10);
        public static float GridPadding = 1f;
        public static float LayerOffset = 1.3333333f;

        public static Camera MainCamera;

        public enum GameType {

            TwelveMorris,
            NineMorris,
            SixMorris,
            ThreeMoriss,
            LaskerMorris
        }

        public static void FindClosestIntersectionHitByRay(Vector3 rayCoordinates, List<Intersection> points) {

            int? bestCandidateIndex = null;
            float? bestDistance = null;

            for (int i = 0; i < points.Count; i++) {

                points[i].occupied = false;
                var candidateDistance = points[i].IntersectsRay(rayCoordinates, MainCamera.Position);
                //Console.WriteLine("circle " + i.ToString() + " : " + candidateDistance.ToString());

                if (candidateDistance == null) {
                    continue;
                }

                if (bestDistance == null) {
                    bestDistance = candidateDistance;
                    bestCandidateIndex = i;
                    continue;
                }
                if (candidateDistance < bestDistance) {
                    bestDistance = candidateDistance;
                    bestCandidateIndex = i;
                }
            }

            if (bestCandidateIndex != null) {
                points[(int)bestCandidateIndex].occupied = true;
            }
        }


    }
}
