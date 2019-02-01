using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mill.Engine;
using Mill.Gameobjects;

namespace Mill {
    public static class Utils {

        public const float DEG2RAD = 3.14159f / 180f;

        public static Engine.Point GridCenter = new Engine.Point(0f, 0f);
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

        public enum PlayerState {

            PlacingMen,
            MovingMen,
            Fly,
            RemoveOpponentsMan,
            GameOver
        }

        public enum GameInfo {

            TurnBegin,
            MenPlaced,
            ThreeMenLined,
            CannotPlace,
            SelectOpponent,
            ManDown
        }

        public static void FindClosestIntersectionHitByRay(Vector3 rayCoordinates, List<Intersection> points, ref Intersection selectedPoint) {

            int? bestCandidateIndex = null;
            float? bestDistance = null;

            for (int i = 0; i < points.Count; i++) {
                var candidateDistance = points[i].IntersectsRay(rayCoordinates, MainCamera.Position);

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

            if (bestCandidateIndex.HasValue) {
                selectedPoint = points[(int)bestCandidateIndex];//.Occupied = true;
            } else {
                selectedPoint = null;
            }
        }

        public static bool HasRayHitGameObject(Vector3 rayCoordinates, Intersection gameobject) {

            var candidateDistance = gameobject.IntersectsRay(rayCoordinates, MainCamera.Position);

            if (candidateDistance.HasValue) {
                return true;
            } else {
                return false;
            }
        }

        public static Vector3 Lerp(Vector3 v0, Vector3 v1, float t) {

            return (1 - t) * v0 + t * v1;
        }


    }
}
