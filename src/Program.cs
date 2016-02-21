using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using TiledSharp;

namespace hexandbox {
    class MyApplication {
        

        static Bitmap bitmap = new Bitmap("../data/Tileset_Hexagonal_PointyTop_60x52_60x80.png");
        static int texture;

        public static TmxLayerTile GetTile(ref TmxMap tmxmap, int x, int y, int layer) {
            return tmxmap.Layers[layer].Tiles[x + y * tmxmap.Width];
        }

        [STAThread]
        public static void Main() {
            using (var game = new GameWindow()) {

                var Map = new TmxMap("..\\data\\map.tmx");
                var tile = GetTile(ref Map, 3, 3, 0);


                game.Load += (sender, e) => {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;

                    GL.ClearColor(Color.MidnightBlue);
                    GL.Enable(EnableCap.Texture2D);

                    GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                    GL.GenTextures(1, out texture);
                    GL.BindTexture(TextureTarget.Texture2D, texture);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                    BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                    bitmap.UnlockBits(data);

                };

                game.Resize += (sender, e) => {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };

                game.UpdateFrame += (sender, e) => {
                    // add game logic, input handling
                    if (game.Keyboard[Key.Escape]) {
                        game.Exit();
                    }
                };

                game.RenderFrame += (sender, e) => {
                



        // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);

                    GL.Clear(ClearBufferMask.ColorBufferBit);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    GL.BindTexture(TextureTarget.Texture2D, texture);
                    GL.Begin(PrimitiveType.Quads);

                    DrawTile(0, 0, 0);

                    GL.End();


                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }

        private static void DrawTile(int x, int y, int sprite) {
            DrawQuad(-0.6f, -0.4f, 1.2f, 0.8f, 0.0f, 0.0f, 1.0f, 1.0f);
        }

        private static void DrawQuad(float x, float y, float width, float height, float tx1, float ty1, float tx2, float ty2) {
            GL.TexCoord2(tx1, ty2); GL.Vertex2(x, y);
            GL.TexCoord2(tx2, ty2); GL.Vertex2(x + width, y);
            GL.TexCoord2(tx2, ty1); GL.Vertex2(x + width, y + height);
            GL.TexCoord2(tx1, ty1); GL.Vertex2(x, y + height);
        }
    }
}
