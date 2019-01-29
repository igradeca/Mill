using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class InteractableObject {

        public Vector3 location { get; set; }
        public float detectionRadius { get; set; }

        public float? IntersectsRay(Vector3 rayDirection, Vector3 rayOrigin) {

            var difference = location - (rayOrigin + rayDirection);
            var differenceLengthSquared = difference.LengthSquared;
            var sphereRadiusSquared = detectionRadius * detectionRadius;

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


    }
}
