using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Libr.GameObjects.Bonuses;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Timer = Libr.Timer;
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
        public BufferObject? VboTextureCoords { get; private set; }
        public float Cell { get; private set; } = 0.1f;
        public int FirstCounter { get; private set; }
        public int SecondCounter { get; private set; }
        private double FrameTime { get; set; }
        private double FrameTimeForBonus { get; set; }
        private double FPS { get; set; }
        private float[] bonusVertexArray;
        private double VarFPS { get; set; }
        public Map Map { get; private set; }
        public Texture Texture { get; private set; }
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
        private readonly TextBlock ScorePlayer1;
        private readonly TextBlock ScorePlayer2;
        public Renderer(string mapString, TextBlock ScorePlayer1, TextBlock ScorePlayer2)
        {
            ShaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            Map = new Map(20, 20, Cell, LoadMapFromFile(mapString));
            Texture = Texture.LoadFromFile(@"data\textures\wall.png");
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
            bonusVertexArray = [0];
            this.ScorePlayer1 = ScorePlayer1;
            this.ScorePlayer2 = ScorePlayer2;
        }
        public void Draw(FrameEventArgs frameEventArgs, Timer timer)
        {
            ShaderProgram?.ActiveProgram();
            Vao?.Activate();
            CreateVAOBackground();
            Vao?.Draw(0,6);
            Vao?.Dispose();
            Vao?.Activate();
            CreateVAO(GetVertWallsArray());
            Vao?.Draw(0, 1000);
            Vao?.Dispose();
            if (GetBonusVertexArray().Length != 0)
            {
                Vao?.Activate();
                CreateVAOBonus(GetBonusVertexArray());
                Vao?.Draw(0, 400);
                Vao?.Dispose();
            }
            Vao?.Activate();
            CreateVAOPlayer(FirstPlayer.GetVertColorArray().ToArray().Concat(SecondPlayer.GetVertColorArray()).ToArray());
            Vao?.Draw(0, 200);
            Vao?.Dispose();
            ShaderProgram?.DeactiveProgram();
            CreateVirtualBonus(frameEventArgs);
            DrawBonusInfo(timer);
            DrawHealthState();
            DrawFuelState();
            DrawReloadLine();
            DrawShoots();
            RestartGame();
            MoveShoots();
        }

        private float[] GetVertWallsArray()
        {
            List<float> result = [];

            foreach (Cell cell in Map.ListWalls)
            {
                float[] cellVertColorArr = [
                    cell.X, cell.Y + cell.Size, 0.0f, 1.0f,1.0f,
                    cell.X, cell.Y, 0.0f, 0.0f,1.0f,
                    cell.X + cell.Size, cell.Y, 0.0f, 0.0f,0.0f,
                    cell.X + cell.Size, cell.Y, 0.0f, 0.0f,0.0f,
                    cell.X + cell.Size, cell.Y + cell.Size, 0.0f, 1.0f,0.0f,
                    cell.X, cell.Y + cell.Size, 0.0f, 1.0f,1.0f
                    ];
                foreach (float vertColor in cellVertColorArr)
                    result.Add(vertColor);
            }

            return result.ToArray();
        }
        public void MoveShoots()
        {
            projectilesToRemove = new List<Projectile>();
            if (FirstPlayer.Projectiles.Count != 0)
                foreach (Projectile projectile in FirstPlayer.Projectiles)
                {
                    projectile.Move(Map.Cells, projectilesToRemove, FirstPlayer,SecondPlayer,1);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                FirstPlayer.Projectiles.Remove(myProjectile);
            if (SecondPlayer.Projectiles.Count != 0)
                foreach (Projectile projectile in SecondPlayer.Projectiles)
                {
                    projectile.Move(Map.Cells, projectilesToRemove, FirstPlayer, SecondPlayer,2);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                SecondPlayer.Projectiles.Remove(myProjectile);
        }

        private float[] GetBonusVertexArray()
        {
            bonusVertexArray = [];
            foreach (VirtualBonus bonus in virtualBonusesList)
            {
                bonusVertexArray = bonusVertexArray.Concat(bonus.GetVertexArray()).ToArray();
            }
            return bonusVertexArray;
        }

        private void CreateVAO(float[] vert_textureMap)
        {

            VboVC = new BufferObject(BufferType.ArrayBuffer);
            VboVC.SetData(vert_textureMap, BufferHint.StaticDraw);

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

        private void CreateVAOBackground()
        {

            VboVC = new BufferObject(BufferType.ArrayBuffer);
            VboVC.SetData([
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f,
             -1.0f, -1.0f, 0.0f, 0.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, -1.0f, 0.0f, 1.0f,0.0f,
             1.0f, 1.0f, 0.0f, 1.0f,1.0f,
             -1.0f, 1.0f, 0.0f, 0.0f,1.0f
            ], BufferHint.StaticDraw);

            int VertexArray = ShaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = ShaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureBackground.Handle);

            Vao = new ArrayObject();
            Vao.Activate();

            Vao.AttachBufer(VboVC);

            Vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            Vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            Vao.Deactivate();
            Vao.DisableAttribAll();
        }

        private void CreateVAOPlayer(float[] vert_texturePl)
        {
            VboVC = new BufferObject(BufferType.ArrayBuffer);
            VboVC.SetData(vert_texturePl, BufferHint.StaticDraw);

            int VertexArray = ShaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = ShaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureTank.Handle);

            Vao = new ArrayObject();
            Vao.Activate();

            Vao.AttachBufer(VboVC);

            Vao.AttribPointer(VertexArray, 3, AttribType.Float, 5 * sizeof(float), 0);
            Vao.AttribPointer(TextureCoordArray, 2, AttribType.Float, 5 * sizeof(float), 3 * sizeof(float));

            Vao.Deactivate();
            Vao.DisableAttribAll();

            ShaderProgram?.SetTexture("aTextureCoord", TextureTank.Handle);
        }

        private void CreateVAOBonus(float[] vert_textureBonus)
        {

            VboVC = new BufferObject(BufferType.ArrayBuffer);
            VboVC.SetData(vert_textureBonus, BufferHint.StaticDraw);

            int VertexArray = ShaderProgram.GetAttribProgram("aPosition");
            int TextureCoordArray = ShaderProgram.GetAttribProgram("aTextureCoord");

            GL.Uniform1(TextureCoordArray, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TextureBonus.Handle);

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

        private void DrawBonusInfo(Timer timer)
        {
            if(FirstPlayer.Speed > 0.0045f)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("speedHigh", [-1.0f, 0.75f, 0.0f, 0.0f, 0.5f, -0.975f, 0.725f, 0.0f, 0.5f, 0.0f, -0.95f, 0.75f, 0.0f, 1.0f, 0.5f, -0.975f, 0.775f, 0.0f, 0.5f, 1.0f, -1.0f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (FirstPlayer.Speed < 0.0035f)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("speedLow", [-1.0f, 0.75f, 0.0f, 0.0f, 0.5f, -0.975f, 0.725f, 0.0f, 0.5f, 0.0f, -0.95f, 0.75f, 0.0f, 1.0f, 0.5f, -0.975f, 0.775f, 0.0f, 0.5f, 1.0f, -1.0f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.Speed > 0.0045f)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("speedHigh", [0.8f, 0.75f, 0.0f, 0.0f, 0.5f, 0.825f, 0.725f, 0.0f, 0.5f, 0.0f, 0.85f, 0.75f, 0.0f, 1.0f, 0.5f, 0.825f, 0.775f, 0.0f, 0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.Speed < 0.0035f)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("speedLow", [0.8f, 0.75f, 0.0f, 0.0f, 0.5f, 0.825f, 0.725f, 0.0f, 0.5f, 0.0f, 0.85f, 0.75f, 0.0f, 1.0f, 0.5f, 0.825f, 0.775f, 0.0f, 0.5f, 1.0f, 0.8f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (FirstPlayer.Damage > 20f)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("damage", [-0.94f, 0.75f, 0.0f, 0.0f, 0.5f, -0.915f, 0.725f, 0.0f, 0.5f, 0.0f, -0.89f, 0.75f, 0.0f, 1.0f, 0.5f, -0.915f, 0.775f, 0.0f, 0.5f, 1.0f, -0.94f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.Damage > 20f)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("damage", [0.86f, 0.75f, 0.0f, 0.0f, 0.5f, 0.885f, 0.725f, 0.0f, 0.5f, 0.0f, 0.91f, 0.75f, 0.0f, 1.0f, 0.5f, 0.885f, 0.775f, 0.0f, 0.5f, 1.0f, 0.86f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (FirstPlayer.TimeReload < 0.45)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("reloadHigh", [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (FirstPlayer.TimeReload > 0.55)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("reloadLow", [-0.88f, 0.75f, 0.0f, 0.0f, 0.5f, -0.855f, 0.725f, 0.0f, 0.5f, 0.0f, -0.83f, 0.75f, 0.0f, 1.0f, 0.5f, -0.855f, 0.775f, 0.0f, 0.5f, 1.0f, -0.88f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.TimeReload < 0.45)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("reloadHigh", [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
            if (SecondPlayer.TimeReload > 0.55)
            {
                ShaderProgram?.ActiveProgram();
                Vao?.Activate();
                CreateVAOBonusInfo("reloadLow", [0.92f, 0.75f, 0.0f, 0.0f, 0.5f, 0.945f, 0.725f, 0.0f, 0.5f, 0.0f, 0.97f, 0.75f, 0.0f, 1.0f, 0.5f, 0.945f, 0.775f, 0.0f, 0.5f, 1.0f, 0.92f, 0.75f, 0.0f, 0.0f, 0.5f]);
                Vao?.DrawPoligon(0, 30);
                Vao?.Dispose();
                ShaderProgram?.DeactiveProgram();
            }
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
            float xStart = FirstPlayer.PointerAndReloadLine()[2];
            float xEnd = FirstPlayer.PointerAndReloadLine()[3];
            float y = FirstPlayer.PointerAndReloadLine()[4];
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
            xStart = SecondPlayer.PointerAndReloadLine()[2];
            xEnd = SecondPlayer.PointerAndReloadLine()[3];
            y = SecondPlayer.PointerAndReloadLine()[4];
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
            if(FirstPlayer.CheckIsDead())
            {
                FirstPlayer = new Player(1);
                SecondPlayer = new Player(2);
                virtualBonusesList = [];
                ScorePlayer2.Text = (int.Parse(ScorePlayer2.Text)+1).ToString();
            }
            if(SecondPlayer.CheckIsDead())
            {
                FirstPlayer = new Player(1);
                SecondPlayer = new Player(2);
                virtualBonusesList = [];
                ScorePlayer1.Text = (int.Parse(ScorePlayer1.Text) + 1).ToString();
            }
        }

        public void CreateVirtualBonus(FrameEventArgs frameEventArgs)
        {
            FrameTimeForBonus += frameEventArgs.Time;
            if (FrameTimeForBonus >= 5)
            {
                virtualBonusesList.Add(new VirtualBonus());
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

        public void OnKeyDown(KeyboardState KeyboardState, Timer timer)
        {
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                FirstPlayer.PlayerMove(Movement.Top, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                FirstPlayer.PlayerMove(Movement.Left, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                FirstPlayer.PlayerMove(Movement.Bottom, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                FirstPlayer.PlayerMove(Movement.Right, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.U))
            {
                SecondPlayer.PlayerMove(Movement.Top, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.J))
            {
                SecondPlayer.PlayerMove(Movement.Bottom, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.H))
            {
                SecondPlayer.PlayerMove(Movement.Left, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer);
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                SecondPlayer.PlayerMove(Movement.Right, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer);
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
