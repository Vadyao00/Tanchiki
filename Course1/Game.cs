using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using Course1;
using Libr;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace Tanchiki
{
    public class GameScene : GameWindow
    {
        private ShaderProgram shaderProgram;
        private BufferObject vboVC;
        private BufferObject vboTextureCoords;
        private ArrayObject vao;

        Texture texture;
        Texture textureTank;

        private MainWindow MainWindowWPF;

        public double value { get; set; }

        
        private float Cell = 0.1f;

        Map map;

        public GameScene(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, MainWindow mainWindowWPF)
      : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.On;
            Title = "Танковая дуэль";
            MainWindowWPF = mainWindowWPF;
            //Size = new Vector2i(1500, 1500);
        }

        private double FrameTime { get; set; }

        private double FPS { get; set; }

        Player FirstPlayer;
        Player SecondPlayer;
        float[] mapArr;

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.LightBlue);
            GL.Enable(EnableCap.CullFace);
            map = new Map(20, 20, Cell, LoadMapFromFile("D:\\Labs\\4sem\\Курсовая работа\\Maps\\map1.txt"));

            shaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            texture = Texture.LoadFromFile("D:\\Labs\\4sem\\Курсовая работа\\textures\\wall.png");
            textureTank = Texture.LoadFromFile("D:\\Labs\\4sem\\Курсовая работа\\textures\\tank.png");
            FirstPlayer = new Player(1);
            mapArr = map.GetVertColorArray();
            SecondPlayer = new Player(2);
            CreateVAO(mapArr);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        }

        protected override void OnUnload()
        {
            vao.Dispose();
            shaderProgram.DeleteProgram();
            base.OnUnload();
            
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //GL.Ortho(-100, 100, -100, 100, -1, 1);
        }
        int counter = 0;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Draw();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            FrameTime += frameEventArgs.Time;
            FPS++;
            if (FrameTime >= 1)
            {
                Title = $"Танковая дуэль - " + FPS;
                FPS = 0;
                FrameTime = 0;
            }

            List<Projectile> projectilesToRemove = new List<Projectile>();
            if (FirstPlayer.projectiles.Count != 0)
                foreach (Projectile projectile in FirstPlayer.projectiles)
                {
                    projectile.Move(map.cells, projectilesToRemove);
                }
            foreach(Projectile myProjectile in projectilesToRemove)
                FirstPlayer.projectiles.Remove(myProjectile);
            if (SecondPlayer.projectiles.Count != 0)
                foreach (Projectile projectile in SecondPlayer.projectiles)
                {
                    
                    projectile.Move(map.cells, projectilesToRemove);
                }
                foreach (Projectile myProjectile in projectilesToRemove)
                SecondPlayer.projectiles.Remove(myProjectile);
            OnKeyDown();
            base.OnUpdateFrame(frameEventArgs);
        }

        private int[,] LoadMapFromFile(string filename)
        {
            try
            {
                string[] lines = File.ReadAllLines(filename);
                int width = lines[0].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                int height = lines.Length;
                int[,] map = new int[width, height];

                for (int y = 0; y < height; y++)
                {
                    string[] values = lines[y].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int x = 0; x < width; x++)
                    {
                        if (int.TryParse(values[x], out int tileValue))
                        {
                            map[x, y] = tileValue;
                        }
                        else
                        {
                            map[x, y] = 0;
                        }
                    }
                }

                return map;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
                return null;
            }
        }

        private void CreateVAO(float[] vert_colors)
        {

            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_colors, BufferHint.StaticDraw);

            vboTextureCoords = new BufferObject(BufferType.ArrayBuffer);
            vboTextureCoords.SetData(vert_colors, BufferHint.StaticDraw);
            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

            vao = new ArrayObject();
            vao.Activate();
            
            vao.AttachBufer(vboVC);
            vao.AttachBufer(vboTextureCoords);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 9 * sizeof(float), 0);
            vao.AttribPointer(ColorArray, 4, AttribType.Float, 9 * sizeof(float), 3 * sizeof(float));
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 9 * sizeof(float), 7 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();

            shaderProgram?.SetTexture("aTextureCoord", texture.Handle);
        }

        private void CreateVAO1(float[] vert_colorPl)
        {
            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_colorPl, BufferHint.StaticDraw);

            vboTextureCoords = new BufferObject(BufferType.ArrayBuffer);
            vboTextureCoords.SetData(vert_colorPl, BufferHint.StaticDraw);
            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureTank.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);
            vao.AttachBufer(vboTextureCoords);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 9 * sizeof(float), 0);
            vao.AttribPointer(ColorArray, 4, AttribType.Float, 9 * sizeof(float), 3 * sizeof(float));
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 9 * sizeof(float), 7 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();

            shaderProgram?.SetTexture("aTextureCoord", textureTank.Handle);
        }

        private void DrawPointDirection()
        {
            float x = FirstPlayer.PointerAndReloadLine()[0];
            float y = FirstPlayer.PointerAndReloadLine()[1];
            GL.PointSize(15);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.Orange);
            GL.Vertex2(x, y);
            GL.End();
            x = SecondPlayer.PointerAndReloadLine()[0];
            y = SecondPlayer.PointerAndReloadLine()[1];
            GL.PointSize(15);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.Orange);
            GL.Vertex2(x, y);
            GL.End();
        }
        private void DrawReloadLine()
        {
            
            float xStart = FirstPlayer.PointerAndReloadLine()[2];
            float xEnd = FirstPlayer.PointerAndReloadLine()[3];
            float y = FirstPlayer.PointerAndReloadLine()[4];
            double timeReload = FirstPlayer.TimeReload;
            float step = (xEnd - xStart)/ (float)(timeReload * 90);
            if (FirstPlayer.IsReloading && counter < (float)(timeReload * 90))
            {
                counter++;
                GL.Color3(Color.Orange);
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(xStart, y);
                GL.Vertex2(xStart + counter * step, y);
                GL.End();
            }
            else
            {
                counter = 0;
                GL.Color3(Color.Gray);
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(xStart, y);
                GL.Vertex2(xEnd, y);
                GL.End();
            }
        }

        private void Draw()
        {
            shaderProgram?.ActiveProgram();
            vao.Activate();
            CreateVAO(mapArr);
            vao.Draw(0, 500);
            vao.Dispose();
            shaderProgram?.DeactiveProgram();
            if (FirstPlayer.IsChanged || SecondPlayer.IsChanged)
            {
                shaderProgram?.ActiveProgram();
                vao.Activate();
                CreateVAO1(FirstPlayer.GetVertColorArray().ToArray().Concat(SecondPlayer.GetVertColorArray()).ToArray());
                vao.Draw(0, 200);
                vao.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            DrawPointDirection();
            DrawReloadLine();
            DrawShoots();
        }

        private void DrawShoots()
        {
            if (FirstPlayer.projectiles.Count != 0)
            {
                foreach (Projectile projectile in FirstPlayer.projectiles)
                {
                    GL.PointSize(20);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex2(projectile.X, projectile.Y);
                    GL.End();
                }
            }
            if(SecondPlayer.projectiles.Count != 0)
            {
                foreach (Projectile projectile in SecondPlayer.projectiles)
                {
                    GL.PointSize(20);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex2(projectile.X, projectile.Y);
                    GL.End();
                }
            }
        }

        private void OnKeyDown()
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                MainWindowWPF.Show();
                Close();
            }
            if(KeyboardState.IsKeyDown(Keys.W))
            {
                FirstPlayer.PlayerMove(Movement.Top, map.GetListCells());
            }
            if(KeyboardState.IsKeyDown(Keys.A))
            {
                FirstPlayer.PlayerMove(Movement.Left, map.GetListCells());
            }
            if( KeyboardState.IsKeyDown(Keys.S))
            {
                FirstPlayer.PlayerMove(Movement.Bottom, map.GetListCells());
            }
            if(KeyboardState.IsKeyDown(Keys.D))
            {
                FirstPlayer.PlayerMove(Movement.Right, map.GetListCells());
            }
            if(KeyboardState.IsKeyDown(Keys.U))
            {
                SecondPlayer.PlayerMove(Movement.Top, map.GetListCells());
            }
            if (KeyboardState.IsKeyDown(Keys.J))
            {
                SecondPlayer.PlayerMove(Movement.Bottom, map.GetListCells());
            }
            if (KeyboardState.IsKeyDown(Keys.H))
            {
                SecondPlayer.PlayerMove(Movement.Left, map.GetListCells());
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                SecondPlayer.PlayerMove(Movement.Right, map.GetListCells());
            }
            if (KeyboardState.IsKeyDown(Keys.V))
            {
                FirstPlayer.Shoot();
            }
            if (KeyboardState.IsKeyDown(Keys.P))
            {
                SecondPlayer.Shoot();
            }
        }
    }
}