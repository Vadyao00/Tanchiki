using System.Windows.Controls;
using Libr;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Timer = Libr.Timer;
namespace Tanchiki
{
    /// <summary>
    /// Представляет игровую сцену, основанную на классе GameWindow.
    /// </summary>
    public class GameScene : GameWindow
    {
        private readonly string mapString;
        private readonly Course1.Menu MainWindowWPF;
        private readonly Timer timer;
        private TextBlock Score;
        private TextBlock ScorePlayer1;
        private TextBlock ScorePlayer2;
        /// <summary>
        /// Инициализирует новый экземпляр класса GameScene.
        /// </summary>
        /// <param name="gameWindowSettings">Настройки окна игры.</param>
        /// <param name="nativeWindowSettings">Настройки нативного окна.</param>
        /// <param name="mainWindowWPF">Главное окно WPF приложения.</param>
        /// <param name="mapString">Строка, представляющая карту игры.</param>
        /// <param name="score">Текстовый блок для отображения общего счета.</param>
        /// <param name="scorePlayer1">Текстовый блок для отображения счета игрока 1.</param>
        /// <param name="scorePlayer2">Текстовый блок для отображения счета игрока 2.</param>
        public GameScene(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, Course1.Menu mainWindowWPF, string mapString, TextBlock score, TextBlock scorePlayer1, TextBlock scorePlayer2)
      : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.On;
            Title = "Танковая дуэль";
            MainWindowWPF = mainWindowWPF;
            Size = new Vector2i(1500, 1500);
            Location = new Vector2i(700, 140);
            this.mapString = mapString;
            timer = new Timer();
            Score = score;
            ScorePlayer1 = scorePlayer1;
            ScorePlayer2 = scorePlayer2;
        }

        private Renderer? renderer;

        /// <summary>
        /// Вызывается при загрузке игровой сцены.
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.AliceBlue);
            GL.Enable(EnableCap.CullFace);
            renderer = new Renderer(mapString,Score,ScorePlayer1,ScorePlayer2);
        }

        /// <summary>
        /// Вызывается при выгрузке игровой сцены.
        /// </summary>
        protected override void OnUnload()
        {
            renderer?.Vao?.Dispose();
            renderer?.ShaderProgram.DeleteProgram();
            base.OnUnload();
        }

        /// <summary>
        /// Вызывается при изменении размера окна.
        /// </summary>
        /// <param name="e">Аргументы события изменения размера окна.</param>
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }

        /// <summary>
        /// Вызывается при отрисовке кадра.
        /// </summary>
        /// <param name="e">Аргументы события отрисовки кадра.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            renderer?.Draw(e);
            Title = renderer?.DrawFPS(e, Title);
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        /// <summary>
        /// Вызывается при обновлении кадра.
        /// </summary>
        /// <param name="frameEventArgs">Аргументы события обновления кадра.</param>
        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            timer.Update();
            renderer?.OnKeyDown(KeyboardState, timer, (float)frameEventArgs.Time);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                MainWindowWPF.Show();
                Close();
            }
            base.OnUpdateFrame(frameEventArgs);
        }
    }
}   