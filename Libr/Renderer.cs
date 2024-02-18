using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using OpenTK.Graphics.OpenGL;

namespace Libr
{
    public class Renderer
    {
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set; }
        public ArrayObject? vao {  get;private set; }
        public ShaderProgram shaderProgram { get; private set; }
        public BufferObject? vboVC { get; private set; }
        public BufferObject? vboTextureCoords { get; private set; }
        public float cell { get; private set; } = 0.1f;
        public float[] mapArr { get; private set; }
        public static int counter { get; private set; } = 0;
        public Map map { get; private set; }
        public Texture texture { get; private set; }
        public Texture textureTank { get; private set; }
        List<Projectile>? projectilesToRemove;
        public Renderer()
        {
            shaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            map = new Map(20, 20, cell, LoadMapFromFile(@"data\maps\map1.txt"));
            mapArr = map.GetVertColorArray();
            texture = Texture.LoadFromFile(@"data\textures\wall.png");
            textureTank = Texture.LoadFromFile(@"data\textures\tank.png");
            FirstPlayer = new Player(1);
            SecondPlayer = new Player(2);
            CreateVAO(mapArr);
        }

        public void Draw()
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
            DrawReloadLine(FirstPlayer, SecondPlayer);
            DrawShoots(FirstPlayer, SecondPlayer);
        }

        public void MoveShoots()
        {
            projectilesToRemove = new List<Projectile>();
            if (FirstPlayer.projectiles.Count != 0)
                foreach (Projectile projectile in FirstPlayer.projectiles)
                {
                    projectile.Move(map.cells, projectilesToRemove);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                FirstPlayer.projectiles.Remove(myProjectile);
            if (SecondPlayer.projectiles.Count != 0)
                foreach (Projectile projectile in SecondPlayer.projectiles)
                {

                    projectile.Move(map.cells, projectilesToRemove);
                }
            foreach (Projectile myProjectile in projectilesToRemove)
                SecondPlayer.projectiles.Remove(myProjectile);
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

        private void CreateVAOPlayer(float[] vert_colorPl)
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

        public void DrawShoots(Player FirstPlayer, Player SecondPlayer)
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
            if (SecondPlayer.projectiles.Count != 0)
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

        public void DrawReloadLine(Player FirstPlayer, Player SecondPlayer)
        {

            float xStart = FirstPlayer.PointerAndReloadLine()[2];
            float xEnd = FirstPlayer.PointerAndReloadLine()[3];
            float y = FirstPlayer.PointerAndReloadLine()[4];
            double timeReload = FirstPlayer.TimeReload;
            float step = (xEnd - xStart) / (float)(timeReload * 90);
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
                throw new Exception();
            }
        }
    }
}
