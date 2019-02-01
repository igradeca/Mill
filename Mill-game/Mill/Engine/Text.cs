using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace Mill.Engine {
    public static class Text {

        public static string FontBitmapFilename = "font.png";
        public static int GlyphsPerLine = 16;
        public static int GlyphLineCount = 16;
        public static int GlyphWidth = 11;
        public static int GlyphHeight = 22;

        public static int CharXSpacing = 11;
        public static int CharYSpacing = 22;
        
        public static int FontTextureID;
        public static int TextureWidth;
        public static int TextureHeight;

        public static int LoadTexture(string filename) {

            using (var bitmap = new Bitmap(filename)) {

                var texId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texId);

                BitmapData data = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height), 
                    ImageLockMode.ReadOnly, 
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                
                GL.TexImage2D(
                    TextureTarget.Texture2D, 
                    0, 
                    PixelInternalFormat.Rgba, 
                    bitmap.Width, 
                    bitmap.Height, 
                    0, 
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                    PixelType.UnsignedByte, 
                    data.Scan0);
                    
                bitmap.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                TextureWidth = bitmap.Width; TextureHeight = bitmap.Height;

                return texId;
            }
        }

        public static void RemoveTexture() {

            GL.DeleteTexture(FontTextureID);
        }


    }
}
