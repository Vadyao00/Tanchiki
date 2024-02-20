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
namespace Tanchiki
{
    public class GameScene : GameWindow
    {
        private string mapString;
        private MainWindow MainWindowWPF;
        public GameScene(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, MainWindow mainWindowWPF, string mapString)
      : base(gameWindowSettings, nativeWindowSettings)
        {
            VSync = VSyncMode.On;
            Title = "Танковая дуэль";
            MainWindowWPF = mainWindowWPF;
            Size = new Vector2i(1500, 1500);
            Location = new Vector2i(700, 140);
            this.mapString = mapString;
        }


        private Renderer? renderer;

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.White);
            GL.Enable(EnableCap.CullFace);
            renderer = new Renderer(mapString);
        }

        protected override void OnUnload()
        {
            renderer?.vao?.Dispose();
            renderer?.shaderProgram.DeleteProgram();
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
            renderer?.Draw(e);
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs frameEventArgs)
        {
            OnKeyDown();
            Title = renderer?.DrawFPS(frameEventArgs, Title);
            renderer?.CreateBonus(frameEventArgs);
            renderer?.MoveShoots();
            base.OnUpdateFrame(frameEventArgs);
        }

        private void OnKeyDown()
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                MainWindowWPF.Show();
                Close();
            }
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                renderer?.FirstPlayer.PlayerMove(Movement.Top, renderer.map.GetListCells(), renderer.bonusList,renderer?.SecondPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                renderer?.FirstPlayer.PlayerMove(Movement.Left, renderer.map.GetListCells(), renderer.bonusList, renderer?.SecondPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                renderer?.FirstPlayer.PlayerMove(Movement.Bottom, renderer.map.GetListCells(), renderer.bonusList, renderer?.SecondPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                renderer?.FirstPlayer.PlayerMove(Movement.Right, renderer.map.GetListCells(), renderer.bonusList, renderer?.SecondPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.U))
            {
                renderer?.SecondPlayer.PlayerMove(Movement.Top, renderer.map.GetListCells(), renderer.bonusList, renderer?.FirstPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.J))
            {
                renderer?.SecondPlayer.PlayerMove(Movement.Bottom, renderer.map.GetListCells(), renderer.bonusList, renderer?.FirstPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.H))
            {
                renderer?.SecondPlayer.PlayerMove(Movement.Left, renderer.map.GetListCells(), renderer.bonusList, renderer?.FirstPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                renderer?.SecondPlayer.PlayerMove(Movement.Right, renderer.map.GetListCells(), renderer.bonusList, renderer?.FirstPlayer);
            }
            if (KeyboardState.IsKeyDown(Keys.V))
            {
                renderer?.FirstPlayer.Shoot();
            }
            if (KeyboardState.IsKeyDown(Keys.P))
            {
                renderer?.SecondPlayer.Shoot();
            }
        }
    }
}