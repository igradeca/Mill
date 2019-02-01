using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Mill.Gameobjects;
using Mill.Engine;

namespace Mill {
    public class DrawUtils {

        public static DrawUtils instance;

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
        }

        private void SetProjectionMatrix() {

            GL.ClearColor(Color.LightGray);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            //GL.Disable(EnableCap.Lighting);

            GL.Viewport(_panelSize);

            var aspectRatio = (float)_panelSize.Width / _panelSize.Height;
            
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f),         // Field of view angle, in radians  // (float)Math.PI / 4    _fov * ((float)Math.PI / 180f)
                aspectRatio,                            // Current window aspect ratio
                0.1f,                                   // Near plane
                1000f);                                 // Far plane
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

            Matrix4 lookAt = Utils.MainCamera.LookAtMatrix;
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
            var rayWorldCoordinates = (Utils.MainCamera.LookAtMatrix.Inverted() * rayEye).Xyz;
            rayWorldCoordinates.Normalize();

            return rayWorldCoordinates;
        }

        public void RenderText(Vector3 position, string text, int charPerLine, float scale = 0.1f) {

            float initXPosition = position.X;

            GL.Begin(PrimitiveType.Quads);

            float u_step = (float)Text.GlyphWidth / (float)Text.TextureWidth;
            float v_step = (float)Text.GlyphHeight / (float)Text.TextureHeight;

            char idx = ' ';
            for (int n = 0; n < text.Length; n++) {
                
                if (text[n] == '\\') {
                    idx = text[n];
                    continue;
                } else if (idx == '\\' && text[n] == 'n') {
                    position.X = initXPosition;
                    position.Y -= Text.CharYSpacing * scale;
                    idx = text[n];
                    continue;
                } else {
                    idx = text[n];
                }
                
                idx = text[n];

                float u = (float)(idx % Text.GlyphsPerLine) * u_step;
                float v = (float)(idx / Text.GlyphsPerLine) * v_step;

                GL.TexCoord2(u, v);
                GL.Vertex3(position.X, position.Y + Text.GlyphHeight * scale, position.Z);
                GL.TexCoord2(u + u_step, v);
                GL.Vertex3(position.X + Text.GlyphWidth * scale, position.Y + Text.GlyphHeight * scale, position.Z);
                GL.TexCoord2(u + u_step, v + v_step);
                GL.Vertex3(position.X + Text.GlyphWidth * scale, position.Y, position.Z);
                GL.TexCoord2(u, v + v_step);
                GL.Vertex3(position.X, position.Y, position.Z);

                position.X += Text.CharXSpacing * scale;

                if (position.X > (Text.CharXSpacing * scale) * charPerLine) {

                    position.X = initXPosition;
                    position.Y -= Text.CharYSpacing * scale;
                }
            }

            GL.End();
        }


    }
}
