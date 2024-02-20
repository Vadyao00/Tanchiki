using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
namespace Libr
{
    public class Renderer
    {
        public List<Bonus> bonusList;
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }
        public ArrayObject? vao {  get;private set; }
        public ShaderProgram shaderProgram { get; private set; }
        public BufferObject? vboVC { get; private set; }
        public BufferObject? vboTextureCoords { get; private set; }
        public float cell { get; private set; } = 0.1f;
        public float[] mapArr { get; private set; }
        public int FirstCounter { get; private set; }
        public int SecondCounter { get; private set; }
        private double FrameTime { get; set; }
        private double FrameTimeForBonus { get; set; }
        private double FPS { get; set; }
        private double varFPS { get; set; }
        public Map map { get; private set; }
        public Texture texture { get; private set; }
        public Texture textureTank { get; private set; }
        private BonusFactory bonusFactory;
        List<Projectile>? projectilesToRemove;
        public Renderer(string mapString)
        {
            shaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            map = new Map(20, 20, cell, LoadMapFromFile(mapString));
            mapArr = map.GetVertColorArray();
            texture = Texture.LoadFromFile(@"data\textures\wall.png");
            textureTank = Texture.LoadFromFile(@"data\textures\tank.png");
            FirstPlayer = new Player(1);
            SecondPlayer = new Player(2);
            CreateVAO(mapArr);
            bonusList = new List<Bonus>();
            bonusFactory = new BonusFactory();
        }

        public void Draw(FrameEventArgs frameEventArgs)
        {
            shaderProgram?.ActiveProgram();
            vao?.Activate();
            CreateVAO(mapArr);
            vao?.Draw(0, 1000);
            vao?.Dispose();
            shaderProgram?.DeactiveProgram();
            if (FirstPlayer.IsChanged || SecondPlayer.IsChanged)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOPlayer(FirstPlayer.GetVertColorArray().ToArray().Concat(SecondPlayer.GetVertColorArray()).ToArray());
                vao?.Draw(0, 200);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            BonusDeactivate(frameEventArgs);
            DrawHealthState();
            DrawFuelState();
            DrawReloadLine();
            DrawShoots();
            DrawBonus();
            RestartGame();
        }

        public void MoveShoots()
        {
            projectilesToRemove = new List<Projectile>();
            if (FirstPlayer.projectiles.Count != 0)
                foreach (Projectile projectile in FirstPlayer.projectiles)
                {
                    projectile.Move(map.cells, projectilesToRemove, FirstPlayer,SecondPlayer,1);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                FirstPlayer.projectiles.Remove(myProjectile);
            if (SecondPlayer.projectiles.Count != 0)
                foreach (Projectile projectile in SecondPlayer.projectiles)
                {
                    projectile.Move(map.cells, projectilesToRemove, FirstPlayer, SecondPlayer,2);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                SecondPlayer.projectiles.Remove(myProjectile);
        }

        private void CreateVAO(float[] vert_colors)
        {

            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_colors, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 9 * sizeof(float), 0);
            vao.AttribPointer(ColorArray, 4, AttribType.Float, 9 * sizeof(float), 3 * sizeof(float));
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 9 * sizeof(float), 7 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void CreateVAOPlayer(float[] vert_colorPl)
        {
            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_colorPl, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureTank.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 9 * sizeof(float), 0);
            vao.AttribPointer(ColorArray, 4, AttribType.Float, 9 * sizeof(float), 3 * sizeof(float));
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 9 * sizeof(float), 7 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();

            shaderProgram?.SetTexture("aTextureCoord", textureTank.Handle);
        }

        private void DrawShoots()
        {
            if (FirstPlayer.projectiles.Count != 0)
            {
                foreach (Projectile projectile in FirstPlayer.projectiles)
                {
                    GL.PointSize(15);
                    GL.Color3(Color.Orange);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex2(projectile.X, projectile.Y);
                    GL.End();
                }
            }
            if (SecondPlayer.projectiles.Count != 0)
            {
                foreach (Projectile projectile in SecondPlayer.projectiles)
                {
                    GL.PointSize(15);
                    GL.Color3(Color.Orange);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex2(projectile.X, projectile.Y);
                    GL.End();
                }
            }
        }

        private void DrawReloadLine()
        {
            float xStart = FirstPlayer.PointerAndReloadLine()[2];
            float xEnd = FirstPlayer.PointerAndReloadLine()[3];
            float y = FirstPlayer.PointerAndReloadLine()[4];
            double timeReload = FirstPlayer.TimeReload;
            float step = (xEnd - xStart) / (float)(timeReload * varFPS);
            if (FirstPlayer.IsReloading && FirstCounter < (float)(timeReload * varFPS))
            {
                FirstCounter++;
                GL.Color3(Color.Orange);
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(xStart, y);
                GL.Vertex2(xStart + FirstCounter * step, y);
                GL.End();
            }
            else
            {
                FirstCounter = 0;
                GL.Color3(Color.Gray);
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(xStart, y);
                GL.Vertex2(xEnd, y);
                GL.End();
            }
            xStart = SecondPlayer.PointerAndReloadLine()[2];
            xEnd = SecondPlayer.PointerAndReloadLine()[3];
            y = SecondPlayer.PointerAndReloadLine()[4];
            timeReload = SecondPlayer.TimeReload;
            step = (xEnd - xStart) / (float)(timeReload * varFPS);
            if (SecondPlayer.IsReloading && SecondCounter < (float)(timeReload * varFPS))
            {
                SecondCounter++;
                GL.Color3(Color.Orange);
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(xStart, y);
                GL.Vertex2(xStart + SecondCounter * step, y);
                GL.End();
            }
            else
            {
                SecondCounter = 0;
                GL.Color3(Color.Gray);
                GL.LineWidth(5);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(xStart, y);
                GL.Vertex2(xEnd, y);
                GL.End();
            }
        }

        public string DrawFPS(FrameEventArgs frameEventArgs, string Title)
        {
            FrameTime += frameEventArgs.Time;
            FPS++;
            if (FrameTime >= 1)
            {
                varFPS = FPS;
                Title = $"Танковая дуэль - " + FPS;
                FPS = 0;
                FrameTime = 0;
            }
            return Title;
        }

        private static int[,] LoadMapFromFile(string filename)
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
                throw new Exception();
            }
        }

        private void DrawHealthState()
        {
            GL.LineWidth(10);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(1.0f, 1.0f);
            GL.Vertex2(0.8f, 1.0f);
            GL.Vertex2(0.8f, 0.9f);
            GL.Vertex2(1.0f, 0.9f);
            GL.Vertex2(1.0f, 1.0f);
            GL.End();
            GL.LineWidth(15);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(1.0f, 1.0f);
            GL.Vertex2(0.8f, 1.0f);
            GL.Vertex2(0.8f, 0.9f);
            GL.Vertex2(1.0f, 0.9f);
            GL.Vertex2(1.0f, 1.0f);
            GL.End();
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Red);
            GL.Vertex2(-1.0f, 0.9f);
            GL.Vertex2(0.002f*FirstPlayer.Health - 1, 0.9f);
            GL.Vertex2(-1.0f, 1.0f);
            GL.Vertex2(0.002f*FirstPlayer.Health - 1, 1.0f);
            GL.End();

            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Red);
            GL.Vertex2(0.8f, 0.9f);
            GL.Vertex2(0.8f + 0.002f * SecondPlayer.Health, 0.9f);
            GL.Vertex2(0.8f, 1.0f);
            GL.Vertex2(0.8f + 0.002f * SecondPlayer.Health, 1.0f);
            GL.End();
        }

        private void DrawFuelState()
        {
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(-1.0f, 0.89f);
            GL.Vertex2(-0.8f, 0.89f);
            GL.Vertex2(-0.8f, 0.79f);
            GL.Vertex2(-1.0f, 0.79f);
            GL.Vertex2(-1.0f, 0.89f);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(1.0f, 0.89f);
            GL.Vertex2(0.8f, 0.89f);
            GL.Vertex2(0.8f, 0.79f);
            GL.Vertex2(1.0f, 0.79f);
            GL.Vertex2(1.0f, 0.89f);
            GL.End();

            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Purple);
            GL.Vertex2(-1.0f, 0.79f);
            GL.Vertex2(0.002f * FirstPlayer.Fuel - 1, 0.79f);
            GL.Vertex2(-1.0f, 0.89f);
            GL.Vertex2(0.002f * FirstPlayer.Fuel - 1, 0.89f);
            GL.End();

            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Purple);
            GL.Vertex2(0.8f, 0.79f);
            GL.Vertex2(0.8f + 0.002f * SecondPlayer.Fuel, 0.79f);
            GL.Vertex2(0.8f, 0.89f);
            GL.Vertex2(0.8f + 0.002f * SecondPlayer.Fuel, 0.89f);
            GL.End();
        }

        private void RestartGame()
        {
            if(FirstPlayer.CheckIsDead() || SecondPlayer.CheckIsDead())
            {
                FirstPlayer = new Player(1);
                SecondPlayer = new Player(2);
            }
        }

        public void CreateBonus(FrameEventArgs frameEventArgs)
        {
            FrameTimeForBonus += frameEventArgs.Time;
            if (FrameTimeForBonus >= 1)
            {
                bonusList.Add(bonusFactory.createBonus());
                FrameTimeForBonus = 0;
            }

            List<Bonus> bonusToRemove = new List<Bonus>();
            foreach (Bonus bonus in bonusList)
            {
                if (bonus.LifeTime >= 15 || bonus.isUsed)
                    bonusToRemove.Add(bonus);
                else bonus.LifeTime += frameEventArgs.Time;
            }
            foreach (Bonus bonus in bonusToRemove)
            {
                bonusList.Remove(bonus);
            }
        }
        private void DrawBonus()
        {
            foreach(Bonus bonus in bonusList)
                if(!bonus.isUsed)
                {
                    int segments = 36;

                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Color3(Color.Red);

                    for (int i = 0; i < segments; i++)
                    {
                        double theta = 2.0f * Math.PI * i / segments;
                        double cx = bonus.X + bonus.Radius * Math.Cos(theta);
                        double cy = bonus.Y + bonus.Radius * Math.Sin(theta);
                        GL.Vertex2(cx, cy);
                    }
                    GL.End();
                }
        }
        private void BonusDeactivate(FrameEventArgs frameEventArgs)
        {
            List<Bonus> bonus = [new SpeedBonus(), new DamageBonus()];
            foreach(Bonus _bonus in bonus)
            {
                if (_bonus is SpeedBonus)
                {
                    if (FirstPlayer.TimeSpeedBonus >= 10)
                    {
                        FirstPlayer.TimeSpeedBonus = 0;
                        _bonus.DeactivateBonus(FirstPlayer);
                    }
                    else { FirstPlayer.TimeSpeedBonus += frameEventArgs.Time; }
                    if (SecondPlayer.TimeSpeedBonus >= 10)
                    {
                        SecondPlayer.TimeSpeedBonus = 0;
                        _bonus.DeactivateBonus(SecondPlayer);
                    }
                    else { SecondPlayer.TimeSpeedBonus += frameEventArgs.Time; }
                }
                if(_bonus is DamageBonus)
                {
                    if (FirstPlayer.TimeDamageBonus >= 10)
                    {
                        FirstPlayer.TimeDamageBonus = 0;
                        _bonus.DeactivateBonus(FirstPlayer);
                    }
                    else { FirstPlayer.TimeDamageBonus += frameEventArgs.Time; }
                    if (SecondPlayer.TimeDamageBonus >= 10)
                    {
                        SecondPlayer.TimeDamageBonus = 0;
                        _bonus.DeactivateBonus(SecondPlayer);
                    }
                    else { SecondPlayer.TimeDamageBonus += frameEventArgs.Time; }
                }
                if (_bonus is ReloadBonus)
                {
                    if (FirstPlayer.TimeReloadBonus >= 10)
                    {
                        FirstPlayer.TimeReloadBonus = 0;
                        _bonus.DeactivateBonus(FirstPlayer);
                    }
                    else { FirstPlayer.TimeReloadBonus += frameEventArgs.Time; }
                    if (SecondPlayer.TimeReloadBonus >= 10)
                    {
                        SecondPlayer.TimeReloadBonus = 0;
                        _bonus.DeactivateBonus(SecondPlayer);
                    }
                    else { SecondPlayer.TimeReloadBonus += frameEventArgs.Time; }
                }
            }

        }
    }
}
