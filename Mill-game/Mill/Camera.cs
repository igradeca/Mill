using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mill.Engine {
    public class Camera {

        public Vector3 Position { get; }
        public Matrix4 LookAtMatrix { get; }

        public Camera() {

            Position = new Vector3(0f, 0f, 5f);

            LookAtMatrix = Matrix4.LookAt(
                Position,                   // Camera position in world space
                Vector3.Zero,               // Target position in world space
                Vector3.UnitY);             // Heads up
        }
        /*
        public Camera(Vector3 position, Vector3 target) {

            Position = position;
            LookAtMatrix = Matrix4.LookAt(
                position, 
                target, 
                Vector3.UnitY);
        }
        */

    }
}
