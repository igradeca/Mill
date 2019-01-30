using Mill.Engine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Gameobjects {
    public class InteractableObject : IGameObject {

        public Vector3 Location { get; set; }
        public float DetectionRadius { get; set; }

        public float? IntersectsRay(Vector3 rayDirection, Vector3 rayOrigin) {

            var difference = Location - (rayOrigin + rayDirection);
            var differenceLengthSquared = difference.LengthSquared;
            var sphereRadiusSquared = DetectionRadius * DetectionRadius;

            if (differenceLengthSquared < sphereRadiusSquared) {
                return 0f;
            }

            var distanceAlongRay = Vector3.Dot(rayDirection, difference);
            if (distanceAlongRay < 0) {
                return null;
            }

            var dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;
            var result = (dist < 0) ? null : distanceAlongRay - (float?)Math.Sqrt(dist);

            return result;
        }

        public void Update(double elapsedTime) {
            throw new NotImplementedException();
        }

        public void Render() {
            throw new NotImplementedException();
        }
    }
}
