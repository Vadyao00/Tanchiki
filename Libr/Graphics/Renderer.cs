﻿using System.Drawing;
using System.IO;
using System.Windows.Controls;
using Libr.GameObjects.Bonuses;
using Libr.GameObjects.Projectilies;
using Libr.Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
namespace Libr
{
    /// <summary>
    /// Класс, представляющий рендеринг игровых объектов.
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// Коллекция виртуальных бонусов.
        /// </summary>
        public List<VirtualBonus> virtualBonusesList;
        /// <summary>
        /// Объект перовго игрока.
        /// </summary>
        public Tank FirstPlayer { get; private set; }
        /// <summary>
        /// Объект второго игрока.
        /// </summary>
        public Tank SecondPlayer { get; private set; }
        /// <summary>
        /// Объект вершинного буфера.
        /// </summary>
        public ArrayObject? Vao {  get;private set; }
        /// <summary>
        /// Объект шейдерной программы.
        /// </summary>
        public ShaderProgram ShaderProgram { get; private set; }
        /// <summary>
        /// Буферный объект вершины.
        /// </summary>
        public BufferObject? VboVC { get; private set; }
        /// <summary>
        /// Счетчик для отображения уровня линии перезарядки первого игрока.
        /// </summary>
        public int FirstCounter { get; private set; }
        /// <summary>
        /// Счетчик для отображения уровня линии перезарядки второго игрока.
        /// </summary>
        public int SecondCounter { get; private set; }
        /// <summary>
        /// Счетчик, отслеживающий время прохождения одной секунды.
        /// </summary>
        private double FrameTime { get; set; }
        /// <summary>
        /// Счетчик, отслеживающий время жизни бонуса.
        /// </summary>
        private double FrameTimeForBonus { get; set; }
        /// <summary>
        /// Счетчик частоты обновления экрана.
        /// </summary>
        private double FPS { get; set; }
        /// <summary>
        /// Текущая частота обновления экрана.
        /// </summary>
        private double VarFPS { get; set; }
        /// <summary>
        /// Объект карты.
        /// </summary>
        public Map Map { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру стены.
        /// </summary>
        public Texture TextureWall { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру танка.
        /// </summary>
        public Texture TextureTank { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру бонуса.
        /// </summary>
        public Texture TextureBonus { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру заднего фона.
        /// </summary>
        public Texture TextureBackground { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру информационной иконки увеличенной скорости движения.
        /// </summary>
        public Texture TextureBonusInfoSpeed { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру информационной иконки урона.
        /// </summary>
        public Texture TextureBonusInfoDamage { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру информационной иконки уменьшенной скорости перезарядки.
        /// </summary>
        public Texture TextureBonusInfoReload { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру информационной иконки увеличенной скорости перезарядуки.
        /// </summary>
        public Texture TextureBonusInfoReloadLow { get; private set; }
        /// <summary>
        /// Объект класса <see cref="Texture"/>, представляющий текстуру информационной иконки уменьшенной скорости движения.
        /// </summary>
        public Texture TextureBonusInfoSpeedLow { get; private set; }
        /// <summary>
        /// Объект класса <see cref="RandomBinusFactory"/>.
        /// </summary>
        private readonly RandomBonusFactory randomBonusFactory;
        /// <summary>
        /// Коллекция снарядов для удаления.
        /// </summary>
        private List<Projectile>? projectilesToRemove;
        /// <summary>
        /// Объект класса <see cref="TextBlock"/>, содержащий счет игроков.
        /// </summary>
        private TextBlock Score;
        /// <summary>
        /// Объект класса <see cref="TextBlock"/>, содержащий счет первого игрока.
        /// </summary>
        private TextBlock ScorePlayer1;
        /// <summary>
        /// Объект класса <see cref="TextBlock"/>, содержащий счет второго игрока.
        /// </summary>
        private TextBlock ScorePlayer2;
        /// <summary>
        /// Конструктор, инициализирующий текстуры игровых объектов и создающий карту и шейдерную программу.
        /// </summary>
        /// <param name="mapString">Путь к файлу с картой.</param>
        /// <param name="score">Объект класса <see cref="TextBlock"/>, содержащий счет игроков.</param>
        /// <param name="scorePlayer1">Объект класса <see cref="TextBlock"/>, содержащий счет первого игрока.</param>
        /// <param name="scorePlayer2">Объект класса <see cref="TextBlock"/>, содержащий счет второго игрока.</param>
        public Renderer(string mapString, TextBlock score, TextBlock scorePlayer1, TextBlock scorePlayer2)
        {
            ShaderProgram = new ShaderProgram(@"data\shaders\shader_base.vert", @"data\shaders\shader_base.frag");
            Map = new Map(20, 20, 0.1f, LoadMapFromFile(mapString));
            TextureWall = Texture.LoadFromFile(@"data\textures\wall.png");
            TextureTank = Texture.LoadFromFile(@"data\textures\tank.png");
            TextureBonus = Texture.LoadFromFile(@"data\textures\bonus.png");
            TextureBackground = Texture.LoadFromFile(@"data\textures\background.png");
            TextureBonusInfoSpeed = Texture.LoadFromFile(@"data\textures\speedBonus.png");
            TextureBonusInfoDamage = Texture.LoadFromFile(@"data\textures\damageBonus.png");
            TextureBonusInfoReload = Texture.LoadFromFile(@"data\textures\reloadBonus.png");
            TextureBonusInfoReloadLow = Texture.LoadFromFile(@"data\textures\reloadBonusLow.png");
            TextureBonusInfoSpeedLow = Texture.LoadFromFile(@"data\textures\speedBonusLow.png");
            FirstPlayer = new Tank(1);
            SecondPlayer = new Tank(2);
            randomBonusFactory = new RandomBonusFactory();
            virtualBonusesList = [];
            Score = score;
            ScorePlayer1 = scorePlayer1;
            ScorePlayer2 = scorePlayer2;
        }
        /// <summary>
        /// Метод, отвечающий за отрисовку и движение игровых объектов.
        /// </summary>
        /// <param name="frameEventArgs">Объект структуры <see cref="FrameEventArgs"/>.</param>
        public void Draw(FrameEventArgs frameEventArgs)
        {
            ShaderProgram?.ActiveProgram();
            Vao?.Activate();
            CreateVAO(VertexGenerator.GetBackgroundVertexArray(), TextureBackground);
            Vao?.Draw(0,6);
            CreateVAO(VertexGenerator.GetWallsVertexArray(Map.ListWalls), TextureWall);
            Vao?.Draw(0, 1000);
            if (VertexGenerator.GetBonusVertexArray(virtualBonusesList).Length != 0)
            {
                CreateVAO(VertexGenerator.GetBonusVertexArray(virtualBonusesList), TextureBonus);
                Vao?.Draw(0, 400);
            }
            CreateVAO(VertexGenerator.GetPlayerVertexArray(FirstPlayer), TextureTank);
            Vao?.DrawPolygon(0, 50);
            CreateVAO(VertexGenerator.GetPlayerVertexArray(SecondPlayer), TextureTank);
            Vao?.DrawPolygon(0, 50);
            Vao?.Dispose();
            ShaderProgram?.DeactiveProgram();
            CreateAndDeleteVirtualBonus(frameEventArgs);
            DrawBonusInfo();
            DrawHealthState();
            DrawFuelState();
            DrawShellState();
            DrawReloadLine();
            DrawShoots();
            RestartGame();
            MoveShoots((float)frameEventArgs.Time);
        }
        /// <summary>
        /// Метод, передвигающий снаряды по карте.
        /// </summary>
        /// <param name="koef">Коэффициент движения снаряда, зависящий от частоты обновления экрана.</param>
        private void MoveShoots(float koef)
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
        /// <summary>
        /// Метод, создающий объект вершинного буфера.
        /// </summary>
        /// <param name="vert_texture">Массив вершин для отрисовки объекта.</param>
        /// <param name="Texture">Текстура объекта.</param>
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

            Vao.AttribPointer(VertexArray, 3, VertexAttribPointerType.Float, 5 * sizeof(float), 0);
            Vao.AttribPointer(TextureCoordArray, 2, VertexAttribPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));

            Vao.Deactivate();
            Vao.DisableAttribAll();
        }
        /// <summary>
        /// Метод, создающий объект вершинного буфера для информационной иконки.
        /// </summary>
        /// <param name="bonusType">Тип бонуса.</param>
        /// <param name="vert_textureBonus">Массив вершин бонуса для отрисовки.</param>
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

            Vao.AttribPointer(VertexArray, 3, VertexAttribPointerType.Float, 5 * sizeof(float), 0);
            Vao.AttribPointer(TextureCoordArray, 2, VertexAttribPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));

            Vao.Deactivate();
            Vao.DisableAttribAll();
        }
        /// <summary>
        /// Метод, отображающий информационные иконки игроков.
        /// </summary>
        private void DrawBonusInfo()
        {
            ShaderProgram?.ActiveProgram();
            Vao?.Activate();
            if (FirstPlayer.Speed > 0.4f)
            {
                CreateVAOBonusInfo("speedHigh", VertexGenerator.GetSpeedFirstVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (FirstPlayer.Speed < 0.3f)
            {
                CreateVAOBonusInfo("speedLow", VertexGenerator.GetSpeedFirstVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (SecondPlayer.Speed > 0.4f)
            {
                CreateVAOBonusInfo("speedHigh", VertexGenerator.GetSpeedSecondVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (SecondPlayer.Speed < 0.3f)
            {
                CreateVAOBonusInfo("speedLow", VertexGenerator.GetSpeedSecondVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (FirstPlayer.Damage > 20f)
            {
                CreateVAOBonusInfo("damage", VertexGenerator.GetDamageFirstVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (SecondPlayer.Damage > 20f)
            {
                CreateVAOBonusInfo("damage", VertexGenerator.GetDamageSecondVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (FirstPlayer.TimeReload < 0.45)
            {
                CreateVAOBonusInfo("reloadHigh", VertexGenerator.GetReloadFirstVertexArray());
                Vao?.DrawPolygon(0, 30);

            }
            if (FirstPlayer.TimeReload > 0.55)
            {
                CreateVAOBonusInfo("reloadLow", VertexGenerator.GetReloadFirstVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (SecondPlayer.TimeReload < 0.45)
            {
                CreateVAOBonusInfo("reloadHigh", VertexGenerator.GetReloadSecondVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            if (SecondPlayer.TimeReload > 0.55)
            {
                CreateVAOBonusInfo("reloadLow", VertexGenerator.GetReloadSecondVertexArray());
                Vao?.DrawPolygon(0, 30);
            }
            Vao?.Dispose();
            ShaderProgram?.DeactiveProgram();
        }
        /// <summary>
        /// Метод, отображающий снаряды на игровом поле.
        /// </summary>
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
        /// <summary>
        /// Метод, отображающий линию перезарядки.
        /// </summary>
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
        /// <summary>
        /// Метод, отображающий частоту обновления экрана.
        /// </summary>
        /// <param name="frameEventArgs">Объект структуры <see cref="FrameEventArgs"/>.</param>
        /// <param name="Title">Строка, содержащая частоту обновления экрана.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод, загружающий карту из файла в массив.
        /// </summary>
        /// <param name="filename">Путь к файлу со строкой.</param>
        /// <returns>Массив цифр, представляющий карту.</returns>
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
        /// <summary>
        /// Метод, отображающий шкалу здороья игроков.
        /// </summary>
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
        /// <summary>
        /// Метод, отображающий шкалу топлива игроков.
        /// </summary>
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
        /// <summary>
        /// Метод, отображающий шкалу снарядов игроков.
        /// </summary>
        private void DrawShellState()
        {
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Orange);
            GL.Vertex2(-0.78f, 0.79f);
            GL.Vertex2(-0.68f, 0.79f);
            GL.Vertex2(-0.78f, 0.002f * FirstPlayer.NumProjectiles + 0.79f);
            GL.Vertex2(-0.68f, 0.002f * FirstPlayer.NumProjectiles + 0.79f);
            GL.End();
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.Color3(Color.Orange);
            GL.Vertex2(0.68f, 0.79f);
            GL.Vertex2(0.78f, 0.79f);
            GL.Vertex2(0.68f, 0.002f * SecondPlayer.NumProjectiles + 0.79f);
            GL.Vertex2(0.78f, 0.002f * SecondPlayer.NumProjectiles + 0.79f);
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
        /// <summary>
        /// Метод, перезапускающий игру.
        /// </summary>
        private void RestartGame()
        {
            if(FirstPlayer.CheckIsDead())
            {
                FirstPlayer = new Tank(1);
                SecondPlayer = new Tank(2);
                virtualBonusesList = [];
                ScorePlayer2.Text = (int.Parse(ScorePlayer2.Text)+1).ToString();
                string scoreString = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
                Score.Text = scoreString;
            }
            if(SecondPlayer.CheckIsDead())
            {
                FirstPlayer = new Tank(1);
                SecondPlayer = new Tank(2);
                virtualBonusesList = [];
                ScorePlayer1.Text = (int.Parse(ScorePlayer1.Text) + 1).ToString();
                string scoreString = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
                Score.Text = scoreString;
            }
        }
        /// <summary>
        /// Метод, создающий и удаляющий виртуальные бонусы на карте.
        /// </summary>
        /// <param name="frameEventArgs">Объект структуры <see cref="FrameEventArgs"/>.</param>
        private void CreateAndDeleteVirtualBonus(FrameEventArgs frameEventArgs)
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
        /// <summary>
        /// Метод, обрабатывающий нажатия на клавиши.
        /// </summary>
        /// <param name="KeyboardState">Объект класса <see cref="KeyboardState"/>.</param>
        /// <param name="timer">Объект класса <see cref="Timer"/>.</param>
        /// <param name="speedKoef">Коэффициент движения танка, зависящий от частоты обновления экрана.</param>
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
                    FirstPlayer.Move(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Right:
                    FirstPlayer.Move(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Bottom:
                    FirstPlayer.Move(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Top:
                    FirstPlayer.Move(firstPlayerMovement, Map.ListWalls, virtualBonusesList, SecondPlayer, randomBonusFactory, timer, speedKoef);
                    break;
            }
            switch (secondPlayerMovement)
            {
                case Movement.Left:
                    SecondPlayer.Move(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Right:
                    SecondPlayer.Move(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Bottom:
                    SecondPlayer.Move(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
                    break;
                case Movement.Top:
                    SecondPlayer.Move(secondPlayerMovement, Map.ListWalls, virtualBonusesList, FirstPlayer, randomBonusFactory, timer, speedKoef);
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
