using System.Drawing;
using System.IO;
using System.Windows.Controls;
using Libr.GameObjects.Bonuses;
using Libr.GameObjects.Projectilies;
using Libr.Utilities;
using Microsoft.Windows.Themes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace Libr
{
    public class Renderer
    {
        public List<VirtualBonus> virtualBonusesList;
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }
        public ArrayObject? Vao {  get;private set; }
        public ShaderProgram ShaderProgram { get; private set; }
        public BufferObject? VboVC { get; private set; }
        private float[] backgroundVertArray;
        public float Wall { get; private set; } = 0.1f;
        public int FirstCounter { get; private set; }
        public int SecondCounter { get; private set; }
        private double FrameTime { get; set; }
        private double FrameTimeForBonus { get; set; }
        private double FPS { get; set; }
        private double VarFPS { get; set; }
        public Map Map { get; private set; }
        public Texture TextureWall { get; private set; }
        public Texture TextureTank { get; private set; }
        public Texture TextureBonus { get; private set; }
        public Texture TextureBackground { get; private set; }
        public Texture TextureBonusInfoSpeed { get; private set; }
        public Texture TextureBonusInfoDamage { get; private set; }
        public Texture TextureBonusInfoReload { get; private set; }
        public Texture TextureBonusInfoReloadLow { get; private set; }
        public Texture TextureBonusInfoSpeedLow { get; private set; }
        private readonly RandomBonusFactory randomBonusFactory;
        private List<Projectile>? projectilesToRemove;
        private TextBlock Score;
        private TextBlock ScorePlayer1;
        private TextBlock ScorePlayer2;
        public Renderer(string mapString, TextBlock score, TextBlock scorePlayer1, TextBlock scorePlayer2)
        {
            ShaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            Map = new Map(20, 20, Wall, LoadMapFromFile(mapString));
            TextureWall = Texture.LoadFromFile(@"data\textures\wall.png");
            TextureTank = Texture.LoadFromFile(@"data\textures\tank.png");
            TextureBonus = Texture.LoadFromFile(@"data\textures\bonus.png");
            TextureBackground = Texture.LoadFromFile(@"data\textures\background.png");
            TextureBonusInfoSpeed = Texture.LoadFromFile(@"data\textures\speedBonus.png");
            TextureBonusInfoDamage = Texture.LoadFromFile(@"data\textures\damageBonus.png");
            TextureBonusInfoReload = Texture.LoadFromFile(@"data\textures\reloadBonus.png");
            TextureBonusInfoReloadLow = Texture.LoadFromFile(@"data\textures\reloadBonusLow.png");
            TextureBonusInfoSpeedLow = Texture.LoadFromFile(@"data\textures\speedBonusLow.png");
            FirstPlayer = new Player(1);
            SecondPlayer = new Player(2);
            randomBonusFactory = new RandomBonusFactory();
            virtualBonusesList = [];
            Score = score;
            backgroundVertArray = [
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f,
             -1.0f, -1.0f, 0.0f, 0.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, 1.0f, 0.0f, 1.0f,1.0f,
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f
            ];
            ScorePlayer1 = scorePlayer1;
            ScorePlayer2 = scorePlayer2;
        }
        public void Draw(FrameEventArgs frameEventArgs)
        {
            ShaderProgram?.ActiveProgram();
            Vao?.Activate();
            CreateVAO(backgroundVertArray, TextureBackground);
            Vao?.Draw(0,6);
            CreateVAO(VertexGenerator.GetWallsVertexArray(Map.ListWalls), TextureWall);
            Vao?.Draw(0, 1000);
            if (VertexGenerator.GetBonusVertexArray(virtualBonusesList).Length != 0)
            {
                CreateVAO(VertexGenerator.GetBonusVertexArray(virtualBonusesList), TextureBonus);
                Vao?.Draw(0, 400);
            }
            CreateVAO(VertexGenerator.GetPlayerVertexArray(FirstPlayer), TextureTank);
            Vao?.DrawPoligon(0, 50);
            CreateVAO(VertexGenerator.GetPlayerVertexArray(SecondPlayer), TextureTank);
            Vao?.DrawPoligon(0, 50);
            Vao?.Dispose();
            ShaderProgram?.DeactiveProgram();
            CreateVirtualBonus(frameEventArgs);
            DrawBonusInfo();
            DrawHealthState();
            DrawFuelState();
            DrawShellState();
            DrawReloadLine();
            DrawShoots();
            RestartGame();
            MoveShoots((float)frameEventArgs.Time);
        }

        public void MoveShoots(float koef)
        {
            projectilesToRemove = new List<Projectile>();
            if (FirstPlayer.Projectiles.Count != 0)
                foreach (Projectile projectile in FirstPlayer.Projectiles)
                {
                    projectile.Move(Map.ListWalls, projectilesToRemove, FirstPlayer,SecondPlayer,1,koef);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                FirstPlayer.Projectiles.Remove(myProjectile);
            if (SecondPlayer.Projectiles.Count != 0)
                foreach (Projectile projectile in SecondPlayer.Projectiles)
                {
                    projectile.Move(Map.ListWalls, projectilesToRemove, FirstPlayer, SecondPlayer,2,koef);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                SecondPlayer.Projectiles.Remove(myProjectile);
        }
        private void CreateVAO(float[] vert_texture, Texture Texture)
        {

            VboVC = new BufferObject(BufferType.ArrayBuffer);
            VboVC.SetData(vert_texture, BufferHint.StaticDraw);

            int VertexArray = ShaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = ShaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture.Handle);

            Vao = new ArrayObject();
            Vao.Activate();

            Vao.AttachBufer(VboVC);

            Vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            Vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            Vao.Deactivate();
            Vao.DisableAttribAll();
        }

        private void CreateVAOBonusInfo(string bonusType,float[] vert_textureBonus)
        {

            VboVC = new BufferObject(BufferType.ArrayBuffer);
            VboVC.SetData(vert_textureBonus, BufferHint.StaticDraw);

            int VertexArray = ShaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = ShaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            switch(bonusType)
            {
                case "speedHigh":
                    GL.BindTexture(TextureTarget.Texture2D, TextureBonusInfoSpeed.Handle);
                    break;
                case "damage":
                    GL.BindTexture(TextureTarget.Texture2D, TextureBonusInfoDamage.Handle);
                    break;
                case "reloadHigh":
                    GL.BindTexture(TextureTarget.Texture2D, TextureBonusInfoReload.Handle);
                    break;
                case "reloadLow":
                    GL.BindTexture(TextureTarget.Texture2D, TextureBonusInfoReloadLow.Handle);
                    break;
                case "speedLow":
                    GL.BindTexture(TextureTarget.Texture2D, TextureBonusInfoSpeedLow.Handle);
                    break;
            }

            Vao = new ArrayObject();
            Vao.Activate();

            Vao.AttachBufer(VboVC);

            Vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            Vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            Vao.Deactivate();
            Vao.DisableAttribAll();
        }

        private void DrawBonusInfo()
        {
            ShaderProgram?.ActiveProgram();
            Vao?.Activate();
            if (FirstPlayer.Speed > 0.4f)
            {
                CreateVAOBonusInfo("speedHigh", [-1.0f, 0.75f, 0.0f, 0.0f, 0.5f, -0.975f, 0.725f, 0.0f, 0.5f, 0.0f, -0.95f, 0.75f, 0.0f, 1.0f, 0.5f, -0.975f, 0.775f, 0.0f, 0.5f, 1.0f, -1.0f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (FirstPlayer.Speed < 0.3f)
            {
                CreateVAOBonusInfo("speedLow", [-1.0f, 0.75f, 0.0f, 0.0f, 0.5f, -0.975f, 0.725f, 0.0f, 0.5f, 0.0f, -0.95f, 0.75f, 0.0f, 1.0f, 0.5f, -0.975f, 0.775f, 0.0f, 0.5f, 1.0f, -1.0f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (SecondPlayer.Speed > 0.4f)
            {
                CreateVAOBonusInfo("speedHigh", [0.8f, 0.75f, 0.0f, 0.0f, 0.5f, 0.825f, 0.725f, 0.0f, 0.5f, 0.0f, 0.85f, 0.75f, 0.0f, 1.0f, 0.5f, 0.825f, 0.775f, 0.0f, 0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (SecondPlayer.Speed < 0.3f)
            {
                CreateVAOBonusInfo("speedLow", [0.8f, 0.75f, 0.0f, 0.0f, 0.5f, 0.825f, 0.725f, 0.0f, 0.5f, 0.0f, 0.85f, 0.75f, 0.0f, 1.0f, 0.5f, 0.825f, 0.775f, 0.0f, 0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (FirstPlayer.Damage > 20f)
            {
                CreateVAOBonusInfo("damage", [-0.94f, 0.75f, 0.0f, 0.0f, 0.5f, -0.915f, 0.725f, 0.0f, 0.5f, 0.0f, -0.89f, 0.75f, 0.0f, 1.0f, 0.5f, -0.915f, 0.775f, 0.0f, 0.5f, 1.0f, -0.94f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (SecondPlayer.Damage > 20f)
            {
                CreateVAOBonusInfo("damage", [0.86f, 0.75f, 0.0f, 0.0f, 0.5f, 0.885f, 0.725f, 0.0f, 0.5f, 0.0f, 0.91f, 0.75f, 0.0f, 1.0f, 0.5f, 0.885f, 0.775f, 0.0f, 0.5f, 1.0f, 0.86f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (FirstPlayer.TimeReload < 0.45)
            {
                CreateVAOBonusInfo("reloadHigh", [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);

            }
            if (FirstPlayer.TimeReload > 0.55)
            {
                CreateVAOBonusInfo("reloadLow", [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (SecondPlayer.TimeReload < 0.45)
            {
                CreateVAOBonusInfo("reloadHigh", [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            if (SecondPlayer.TimeReload > 0.55)
            {
                CreateVAOBonusInfo("reloadLow", [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
            }
            Vao?.Dispose();
            ShaderProgram?.DeactiveProgram();
        }

        private void DrawShoots()
        {
            if (FirstPlayer.Projectiles.Count != 0)
            {
                foreach (Projectile projectile in FirstPlayer.Projectiles)
                {
                    GL.PointSize(15);
                    GL.Color3(Color.Orange);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex2(projectile.X, projectile.Y);
                    GL.End();
                }
            }
            if (SecondPlayer.Projectiles.Count != 0)
            {
                foreach (Projectile projectile in SecondPlayer.Projectiles)
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
            float[] reloadLineVertexArrayFirstPlayer = VertexGenerator.GetReloadLineVertexArray(FirstPlayer);
            float xStart = reloadLineVertexArrayFirstPlayer[0];
            float xEnd = reloadLineVertexArrayFirstPlayer[1];
            float y = reloadLineVertexArrayFirstPlayer[2];
            double timeReload = FirstPlayer.TimeReload;
            float step = (xEnd - xStart) / (float)(timeReload * VarFPS);
            if (FirstPlayer.IsReloading && FirstCounter < (float)(timeReload * VarFPS))
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
            float[] reloadLineVertexArraySecondPlayer = VertexGenerator.GetReloadLineVertexArray(SecondPlayer);
            xStart = reloadLineVertexArraySecondPlayer[0];
            xEnd = reloadLineVertexArraySecondPlayer[1];
            y = reloadLineVertexArraySecondPlayer[2];
            timeReload = SecondPlayer.TimeReload;
            step = (xEnd - xStart) / (float)(timeReload * VarFPS);
            if (SecondPlayer.IsReloading && SecondCounter < (float)(timeReload * VarFPS))
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
                VarFPS = FPS;
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
                throw new Exception($"Ошибка чтения файла: {ex.Message}");
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

        private void DrawShellState()
        {
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Orange);
            GL.Vertex2(-0.78f, 0.79f);
            GL.Vertex2(-0.68f, 0.79f);
            GL.Vertex2(-0.78f, 0.002f * FirstPlayer.NumShells + 0.79f);
            GL.Vertex2(-0.68f, 0.002f * FirstPlayer.NumShells + 0.79f);
            GL.End();
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Orange);
            GL.Vertex2(0.68f, 0.79f);
            GL.Vertex2(0.78f, 0.79f);
            GL.Vertex2(0.68f, 0.002f * SecondPlayer.NumShells + 0.79f);
            GL.Vertex2(0.78f, 0.002f * SecondPlayer.NumShells + 0.79f);
            GL.End();
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(-0.78f, 0.99f);
            GL.Vertex2(-0.68f, 0.99f);
            GL.Vertex2(-0.68f, 0.79f);
            GL.Vertex2(-0.78f, 0.79f);
            GL.Vertex2(-0.78f, 0.99f);
            GL.End();
            GL.LineWidth(5);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Black);
            GL.Vertex2(0.78f, 0.99f);
            GL.Vertex2(0.68f, 0.99f);
            GL.Vertex2(0.68f, 0.79f);
            GL.Vertex2(0.78f, 0.79f);
            GL.Vertex2(0.78f, 0.99f);
            GL.End();
        }

        private void RestartGame()
        {
            if(FirstPlayer.CheckIsDead())
            {
                FirstPlayer = new Player(1);
                SecondPlayer = new Player(2);
                virtualBonusesList = [];
                ScorePlayer2.Text = (int.Parse(ScorePlayer2.Text)+1).ToString();
                string scoreString = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
                Score.Text = scoreString;
            }
            if(SecondPlayer.CheckIsDead())
            {
                FirstPlayer = new Player(1);
                SecondPlayer = new Player(2);
                virtualBonusesList = [];
                ScorePlayer1.Text = (int.Parse(ScorePlayer1.Text) + 1).ToString();
                string scoreString = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
                Score.Text = scoreString;
            }
        }

        public void CreateVirtualBonus(FrameEventArgs frameEventArgs)
        {
            FrameTimeForBonus += frameEventArgs.Time;
            if (FrameTimeForBonus >= 4)
            {
                virtualBonusesList.Add(new VirtualBonus(Map.ListWalls));
                FrameTimeForBonus = 0;
            }

            List<VirtualBonus> bonusToRemove = [];
            foreach (VirtualBonus bonus in virtualBonusesList)
            {
                if (bonus.LifeTime >= 15 || bonus.IsUsed)
                    bonusToRemove.Add(bonus);
                else bonus.LifeTime += frameEventArgs.Time;
            }
            foreach (VirtualBonus bonus in bonusToRemove)
            {
                virtualBonusesList.Remove(bonus);
            }
        }
        public void OnKeyDown(KeyboardState KeyboardState, Timer timer, float speedKoef)
        {
            Movement? firstPlayerMovement = null;
            Movement? secondPlayerMovement = null;
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                firstPlayerMovement = Movement.Top;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                firstPlayerMovement = Movement.Left;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                firstPlayerMovement = Movement.Bottom;
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                firstPlayerMovement = Movement.Right;
            }
            if (KeyboardState.IsKeyDown(Keys.I))
            {
                secondPlayerMovement = Movement.Top;
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                secondPlayerMovement = Movement.Bottom;
            }
            if (KeyboardState.IsKeyDown(Keys.J))
            {
                secondPlayerMovement = Movement.Left;
            }
            if (KeyboardState.IsKeyDown(Keys.L))
            {
                secondPlayerMovement = Movement.Right;
            }
            switch(firstPlayerMovement)
            {
                case Movement.Left:
                    FirstPlayer.PlayerMove(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Right:
                    FirstPlayer.PlayerMove(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Bottom:
                    FirstPlayer.PlayerMove(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Top:
                    FirstPlayer.PlayerMove(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
            }
            switch (secondPlayerMovement)
            {
                case Movement.Left:
                    SecondPlayer.PlayerMove(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Right:
                    SecondPlayer.PlayerMove(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Bottom:
                    SecondPlayer.PlayerMove(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Top:
                    SecondPlayer.PlayerMove(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
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
