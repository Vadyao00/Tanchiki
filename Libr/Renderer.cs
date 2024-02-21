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
        private float[] bonusVertexArray;
        private double varFPS { get; set; }
        public Map map { get; private set; }
        public Texture texture { get; private set; }
        public Texture textureTank { get; private set; }
        public Texture textureBonus { get; private set; }
        public Texture textureBackground { get; private set; }
        public Texture textureBonusInfoSpeed { get; private set; }
        public Texture textureBonusInfoDamage { get; private set; }
        public Texture textureBonusInfoReload { get; private set; }

        private BonusFactory bonusFactory;
        List<Projectile>? projectilesToRemove;
        public Renderer(string mapString)
        {
            shaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            map = new Map(20, 20, cell, LoadMapFromFile(mapString));
            mapArr = map.GetVertColorArray();
            texture = Texture.LoadFromFile(@"data\textures\wall.png");
            textureTank = Texture.LoadFromFile(@"data\textures\tank.png");
            textureBonus = Texture.LoadFromFile(@"data\textures\bonus.png");
            textureBackground = Texture.LoadFromFile(@"data\textures\background.png");
            textureBonusInfoSpeed = Texture.LoadFromFile(@"data\textures\speedBonus.png");
            textureBonusInfoDamage = Texture.LoadFromFile(@"data\textures\damageBonus.png");
            textureBonusInfoReload = Texture.LoadFromFile(@"data\textures\reloadBonus.png");
            FirstPlayer = new Player(1);
            SecondPlayer = new Player(2);
            CreateVAO(mapArr);
            bonusList = new List<Bonus>();
            bonusFactory = new BonusFactory();
            bonusVertexArray = [0];
        }

        public void Draw(FrameEventArgs frameEventArgs)
        {
            shaderProgram?.ActiveProgram();
            vao?.Activate();
            CreateVAOBackground();
            vao?.Draw(0,6);
            vao?.Dispose();
            shaderProgram?.DeactiveProgram();
            shaderProgram?.ActiveProgram();
            vao?.Activate();
            CreateVAO(mapArr);
            vao?.Draw(0, 1000);
            vao?.Dispose();
            shaderProgram?.DeactiveProgram();
            if (GetBonusVertexArray().Length != 0)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonus(GetBonusVertexArray());
                vao?.Draw(0, 400);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            shaderProgram?.ActiveProgram();
            vao?.Activate();
            CreateVAOPlayer(FirstPlayer.GetVertColorArray().ToArray().Concat(SecondPlayer.GetVertColorArray()).ToArray());
            vao?.Draw(0, 200);
            vao?.Dispose();
            shaderProgram?.DeactiveProgram();
            BonusDeactivate(frameEventArgs);
            DrawHealthState();
            DrawFuelState();
            DrawBonusInfo();
            DrawReloadLine();
            DrawShoots();
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

        private float[] GetBonusVertexArray()
        {
            bonusVertexArray = [];
            foreach (Bonus bonus in bonusList)
            {
                bonusVertexArray = bonusVertexArray.Concat(bonus.GetVertexArray()).ToArray();
            }
            return bonusVertexArray;
        }

        private void CreateVAO(float[] vert_textureMap)
        {

            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_textureMap, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void CreateVAOBackground()
        {

            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData([
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f,
             -1.0f, -1.0f, 0.0f, 0.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, 1.0f, 0.0f, 1.0f,1.0f,
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f
            ], BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureBackground.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void CreateVAOPlayer(float[] vert_texturePl)
        {
            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_texturePl, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureTank.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();

            shaderProgram?.SetTexture("aTextureCoord", textureTank.Handle);
        }

        private void CreateVAOBonus(float[] vert_textureBonus)
        {

            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_textureBonus, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureBonus.Handle);

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void CreateVAOBonusInfo(string bonusType,float[] vert_textureBonus)
        {

            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_textureBonus, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = shaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            switch(bonusType)
            {
                case "speed":
                    GL.BindTexture(TextureTarget.Texture2D, textureBonusInfoSpeed.Handle);
                    break;
                case "damage":
                    GL.BindTexture(TextureTarget.Texture2D, textureBonusInfoDamage.Handle);
                    break;
                case "reload":
                    GL.BindTexture(TextureTarget.Texture2D, textureBonusInfoReload.Handle);
                    break;
            }

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBufer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void DrawBonusInfo()
        {
            if(FirstPlayer.isSpeedBonusActive)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonusInfo("speed",[-1.0f,0.75f,0.0f,  0.0f,0.5f,  -0.975f,0.725f,0.0f,  0.5f,0.0f,  -0.95f,0.75f,0.0f,  1.0f,0.5f,  -0.975f,0.775f,0.0f,  0.5f, 1.0f  , -1.0f, 0.75f, 0.0f, 0.0f, 0.5f]);
                vao?.DrawPoligon(0, 30);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.isSpeedBonusActive)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonusInfo("speed", [0.8f, 0.75f, 0.0f,  0.0f, 0.5f,  0.825f,0.725f, 0.0f,  0.5f, 0.0f,  0.85f, 0.75f, 0.0f,  1.0f, 0.5f,  0.825f, 0.775f, 0.0f,  0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f]);
                vao?.DrawPoligon(0, 30);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            if(FirstPlayer.isDamageBonusActive)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonusInfo("damage", [-0.94f, 0.75f, 0.0f, 0.0f, 0.5f, -0.915f, 0.725f, 0.0f, 0.5f, 0.0f, -0.89f, 0.75f, 0.0f, 1.0f, 0.5f, -0.915f, 0.775f, 0.0f, 0.5f, 1.0f, -0.94f, 0.75f, 0.0f, 0.0f, 0.5f]);
                vao?.DrawPoligon(0, 30);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.isDamageBonusActive)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonusInfo("damage", [0.86f, 0.75f, 0.0f, 0.0f, 0.5f, 0.885f, 0.725f, 0.0f, 0.5f, 0.0f, 0.91f, 0.75f, 0.0f, 1.0f, 0.5f, 0.885f, 0.775f, 0.0f, 0.5f, 1.0f, 0.86f, 0.75f, 0.0f, 0.0f, 0.5f]);
                vao?.DrawPoligon(0, 30);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            if (FirstPlayer.isReloadBonusActive)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonusInfo("reload", [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f]);
                vao?.DrawPoligon(0, 30);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.isReloadBonusActive)
            {
                shaderProgram?.ActiveProgram();
                vao?.Activate();
                CreateVAOBonusInfo("reload", [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f]);
                vao?.DrawPoligon(0, 30);
                vao?.Dispose();
                shaderProgram?.DeactiveProgram();
            }
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
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(1.0f, 0.998f);
            GL.Vertex2(0.8f, 0.998f);
            GL.Vertex2(0.8f, 0.898f);
            GL.Vertex2(1.0f, 0.898f);
            GL.Vertex2(1.0f, 0.998f);
            GL.End();
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(-1.0f, 0.998f);
            GL.Vertex2(-0.8f, 0.998f);
            GL.Vertex2(-0.8f, 0.898f);
            GL.Vertex2(-1.0f, 0.898f);
            GL.Vertex2(-1.0f, 0.998f);
            GL.End();
        }

        private void DrawFuelState()
        {

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
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(-1.0f, 0.89f);
            GL.Vertex2(-0.8f, 0.89f);
            GL.Vertex2(-0.8f, 0.79f);
            GL.Vertex2(-1.0f, 0.79f);
            GL.Vertex2(-1.0f, 0.89f);
            GL.End();
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(1.0f, 0.89f);
            GL.Vertex2(0.8f, 0.89f);
            GL.Vertex2(0.8f, 0.79f);
            GL.Vertex2(1.0f, 0.79f);
            GL.Vertex2(1.0f, 0.89f);
            GL.End();
        }

        private void RestartGame()
        {
            if(FirstPlayer.CheckIsDead() || SecondPlayer.CheckIsDead())
            {
                FirstPlayer = new Player(1);
                SecondPlayer = new Player(2);
                bonusList = new List<Bonus>();
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
        private void BonusDeactivate(FrameEventArgs frameEventArgs)
        {
            List<Bonus> bonus = [new SpeedBonus(), new DamageBonus(), new ReloadBonus()];
            foreach(Bonus _bonus in bonus)
            {
                if (_bonus is SpeedBonus)
                {   if (FirstPlayer.isSpeedBonusActive)
                    {
                        if (FirstPlayer.TimeSpeedBonus >= 10)
                        {
                            FirstPlayer.TimeSpeedBonus = 0;
                            _bonus.DeactivateBonus(FirstPlayer);
                            FirstPlayer.isSpeedBonusActive = false;
                        }
                        else { FirstPlayer.TimeSpeedBonus += frameEventArgs.Time; }
                    }
                    if (SecondPlayer.isSpeedBonusActive)
                    {
                        if (SecondPlayer.TimeSpeedBonus >= 10)
                        {
                            SecondPlayer.TimeSpeedBonus = 0;
                            _bonus.DeactivateBonus(SecondPlayer);
                            SecondPlayer.isSpeedBonusActive = false;
                        }
                        else { SecondPlayer.TimeSpeedBonus += frameEventArgs.Time; }
                    }
                }
                if(_bonus is DamageBonus)
                {
                    if (FirstPlayer.isDamageBonusActive)
                    {
                        if (FirstPlayer.TimeDamageBonus >= 10)
                        {
                            FirstPlayer.TimeDamageBonus = 0;
                            _bonus.DeactivateBonus(FirstPlayer);
                            FirstPlayer.isDamageBonusActive = false;
                        }
                        else { FirstPlayer.TimeDamageBonus += frameEventArgs.Time; }
                    }
                    if (SecondPlayer.isDamageBonusActive)
                    {
                        if (SecondPlayer.TimeDamageBonus >= 10)
                        {
                            SecondPlayer.TimeDamageBonus = 0;
                            _bonus.DeactivateBonus(SecondPlayer);
                            SecondPlayer.isDamageBonusActive = false;
                        }
                        else { SecondPlayer.TimeDamageBonus += frameEventArgs.Time; }
                    }
                    
                }
                if (_bonus is ReloadBonus)
                {
                    if (FirstPlayer.isReloadBonusActive)
                    {
                        if (FirstPlayer.TimeReloadBonus >= 10)
                        {
                            FirstPlayer.TimeReloadBonus = 0;
                            _bonus.DeactivateBonus(FirstPlayer);
                            FirstPlayer.isReloadBonusActive = false;
                        }
                        else { FirstPlayer.TimeReloadBonus += frameEventArgs.Time; }
                    }
                    if (SecondPlayer.isReloadBonusActive)
                    {
                        if (SecondPlayer.TimeReloadBonus >= 10)
                        {
                            SecondPlayer.TimeReloadBonus = 0;
                            _bonus.DeactivateBonus(SecondPlayer);
                            SecondPlayer.isReloadBonusActive = false;
                        }
                        else { SecondPlayer.TimeReloadBonus += frameEventArgs.Time; }
                    }
                }
            }

        }
    }
}
