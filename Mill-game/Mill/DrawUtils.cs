using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Mill.Engine {
    public class DrawUtils {

        public static DrawUtils instance;

        public Camera MainCamera;

        private Size _panelSize;
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;

        public DrawUtils(Size rect) {

            if (instance != null) {
                throw new ArgumentException("DrawUtils instance already exists.", "DrawUtils");
            } else {
                instance = this;
            }

            _panelSize = rect;
            SetProjectionMatrix();
            MainCamera = new Camera();
        }

        private void SetProjectionMatrix() {

            GL.ClearColor(Color.LightGray);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);

            GL.Viewport(_panelSize);

            var aspectRatio = (float)_panelSize.Width / _panelSize.Height;
            
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f),         // Field of view angle, in radians
                aspectRatio,                            // Current window aspect ratio
                0.001f,                                 // Near plane
                100f);                                  // Far plane
        }

        public void InitGL() {

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _projectionMatrix);
        }

        public void OnRender() {

            //GL.ClearColor(Color.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            Matrix4 lookAt = MainCamera.LookAtMatrix;
            GL.LoadMatrix(ref lookAt);
        }

        public Vector3 CameraToWorldPosition(float mouseX, float mouseY) {

            var x = (2f * mouseX) / _panelSize.Width;
            var y = (2f * mouseY) / _panelSize.Height;
            var z = 1f;
            var rayNormalizedDeviceCoordinates = new Vector3(x, y, z);
            //Console.WriteLine(x + " " + y);

            // 4D homogeneous clip coordinates
            var rayClip = new Vector4(rayNormalizedDeviceCoordinates.X, rayNormalizedDeviceCoordinates.Y, -1f, 1f);

            // 4D eye (camera) coordinates
            var rayEye = _projectionMatrix.Inverted() * rayClip;
            rayEye = new Vector4(rayEye.X, rayEye.Y, -1f, 0f);

            // 4D world coordinates
            var rayWorldCoordinates = (MainCamera.LookAtMatrix.Inverted() * rayEye).Xyz;
            rayWorldCoordinates.Normalize();

            return rayWorldCoordinates;
        }

        public void FindClosestIntersectionHitByRay(Vector3 rayCoordinates, List<Intersection> circles) {

            int? bestCandidateIndex = null;
            float? bestDistance = null;

            for (int i = 0; i < circles.Count; i++) {

                circles[i].occupied = false;
                var candidateDistance = circles[i].IntersectsRay(rayCoordinates, MainCamera.Position);

                Console.WriteLine("circle " + i.ToString() + " : " + candidateDistance.ToString());

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
                circles[(int)bestCandidateIndex].occupied = true;
            }
        }




    }
}
