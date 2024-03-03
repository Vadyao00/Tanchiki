using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using Course1;
using Libr;
using OpenTK.Input;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Timer = Libr.GameObjects.Bonus_Management.Timer;
namespace Tanchiki
{
    public class GameScene : GameWindow
    {
        private readonly string mapString;
        private readonly MainWindow MainWindowWPF;
        private readonly Timer timer;
        private TextBlock ScorePlayer1;
        private TextBlock ScorePlayer2;
        public GameScene(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, MainWindow mainWindowWPF, string mapString, TextBlock ScorePlayer1, TextBlock ScorePlayer2)
      : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.On;
            Title = "Танковая дуэль";
            MainWindowWPF = mainWindowWPF;
            Size = new Vector2i(1500, 1500);
            Location = new Vector2i(700, 140);
            this.mapString = mapString;
            timer = new Timer();
            this.ScorePlayer1 = ScorePlayer1;
            this.ScorePlayer2 = ScorePlayer2;
        }


        private Renderer? renderer;

        protected override void OnLoad()
        {
            base.OnLoad();
            timer.Start();
            GL.ClearColor(Color4.AliceBlue);
            GL.Enable(EnableCap.CullFace);
            renderer = new Renderer(mapString, ScorePlayer1, ScorePlayer2);
        }

        protected override void OnUnload()
        {
            renderer?.Vao?.Dispose();
            renderer?.ShaderProgram.DeleteProgram();
            timer.Stop();
            base.OnUnload();
            
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            timer.Update();
            renderer?.Draw(e,timer);
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            renderer?.OnKeyDown(KeyboardState,timer);
            Title = renderer?.DrawFPS(frameEventArgs, Title);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                MainWindowWPF.Show();
                Close();
            }
            base.OnUpdateFrame(frameEventArgs);
        }

        
    }
}