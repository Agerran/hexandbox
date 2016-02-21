using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using TiledSharp;
using hexandbox;

namespace hexandbox {
    static class MyApplication {

        static Bitmap bitmap = new Bitmap("../data/Tileset_Hexagonal_PointyTop_60x52_60x80.png");
        static int texture;

        static GameWindow game;

        [STAThread]
        public static void Main() {
            using (game = new GameWindow()) {

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

                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    GL.Enable(EnableCap.Blend);
                    GL.BindTexture(TextureTarget.Texture2D, texture);
                    GL.Begin(PrimitiveType.Quads);

                    var Map = ResourceManager.Map;

                    for (int iy = 0; iy < Map.Height; iy++) {
                        int dX = iy % 2 == 0 ? 0 : Map.TileWidth / 2;
                        for (int ix = 0; ix < Map.Width; ix++) {
                            DrawTile(dX + ix * Map.TileWidth, iy * Map.HexSideLength.Value, Map.TileWidth, Map.TileHeight, ResourceManager.GetTile(ix, iy, 0).Gid);
                        }
                    }

                    GL.End();
                    
                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }

        private static float sceneXtoScreenX(int x) {
            
            return 2 * (float)x / (float)game.Width - 1.0f;
        }

        private static float sceneYtoScreenY(int y) {
            return 1.0f - 2 * (float)y / (float)game.Height;
        }

        private static void DrawTile(int x, int y, int width, int height, int spriteId) {
            SpriteTextureDTO sprite = ResourceManager.GetTexture(spriteId);
            DrawQuad(x, y, width, height, sprite.x0, sprite.y0, sprite.x1, sprite.y1);
            //DrawQuad(sceneXtoScreenX(0), sceneYtoScreenY(0), sceneXtoScreenX(100), sceneYtoScreenY(100), sprite.x0, sprite.y0, sprite.x1, sprite.y1);
        }

        private static void DrawQuad(int x, int y, int width, int height, float tx1, float ty1, float tx2, float ty2) {
            GL.TexCoord2(tx1, ty2); GL.Vertex2(sceneXtoScreenX(x), sceneYtoScreenY(y + height));
            GL.TexCoord2(tx2, ty2); GL.Vertex2(sceneXtoScreenX(x + width), sceneYtoScreenY(y + height));
            GL.TexCoord2(tx2, ty1); GL.Vertex2(sceneXtoScreenX(x + width), sceneYtoScreenY(y));
            GL.TexCoord2(tx1, ty1); GL.Vertex2(sceneXtoScreenX(x), sceneYtoScreenY(y));
        }
    }

}
