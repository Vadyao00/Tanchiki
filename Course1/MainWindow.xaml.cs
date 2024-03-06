using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tanchiki;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Windows.Media.Effects;

namespace Course1
{
    public partial class MainWindow : Window
    {
        private string mapString = @"data\maps\map1.txt";
        private bool IsMap1Clicked = false;
        private bool IsMap2Clicked = false;
        private bool IsMap3Clicked = false;
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;

        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                WindowState = OpenTK.Windowing.Common.WindowState.Normal,
                WindowBorder = WindowBorder.Resizable,
                StartFocused = true,
                StartVisible = true,
                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,
            };

            using (GameScene gameScene = new(GameWindowSettings.Default, nativeWindowSettings,this,mapString, ScorePlayer1, ScorePlayer2))
            {
                Hide();
                gameScene.Run();
            }
        }

        private void Map1_Click(object sender, RoutedEventArgs e)
        {
            IsMap1Clicked = true;
            IsMap3Clicked = false;
            IsMap2Clicked = false;
            mapString = @"data\maps\map1.txt";
            Button button = (Button)sender;

            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Green;
            dropShadowEffect.ShadowDepth = 8;
            dropShadowEffect.Opacity = 0.8;
            button.Effect = dropShadowEffect;

            map2Button.BorderThickness = new Thickness(1);
            map2Button.Effect = null;
            map3Button.BorderThickness = new Thickness(1);
            map3Button.Effect = null;
        }
        private void Map2_Click(object sender, RoutedEventArgs e)
        {
            IsMap2Clicked = true;
            IsMap1Clicked = false;
            IsMap3Clicked = false;
            mapString = @"data\maps\map2.txt";
            Button button = (Button)sender;

            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Green;
            dropShadowEffect.ShadowDepth = 8;
            dropShadowEffect.Opacity = 0.8;
            button.Effect = dropShadowEffect;

            map1Button.BorderThickness = new Thickness(1);
            map1Button.Effect = null;
            map3Button.BorderThickness = new Thickness(1);
            map3Button.Effect = null;
        }
        private void Map3_Click(object sender, RoutedEventArgs e)
        {
            IsMap3Clicked = true;
            IsMap1Clicked = false;
            IsMap2Clicked = false;
            mapString = @"data\maps\map3.txt";
            Button button = (Button)sender;

            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Green;
            dropShadowEffect.ShadowDepth = 8;
            dropShadowEffect.Opacity = 0.8;
            button.Effect = dropShadowEffect;

            map2Button.BorderThickness = new Thickness(1);
            map2Button.Effect = null;
            map1Button.BorderThickness = new Thickness(1);
            map1Button.Effect = null;
        }

        private void ButtonMap_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;

            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Green;
            dropShadowEffect.ShadowDepth = 8;
            dropShadowEffect.Opacity = 0.8;
            button.Effect = dropShadowEffect;
        }
        private void ButtonMap1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsMap1Clicked) return;
            Button button = (Button)sender;
            button.BorderThickness = new Thickness(1);
            button.Effect = null;
        }
        private void ButtonMap2_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsMap2Clicked) return;
            Button button = (Button)sender;
            button.BorderThickness = new Thickness(1);
            button.Effect = null;
        }
        private void ButtonMap3_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsMap3Clicked) return;
            Button button = (Button)sender;
            button.BorderThickness = new Thickness(1);
            button.Effect = null;
        }

        private void ButtonInformation_Click(object sender, RoutedEventArgs e)
        {
            InformationWindow infoWindow = new InformationWindow();
            infoWindow.Show();
        }
        private void ButtonResetScore_Click(object sender, RoutedEventArgs e)
        {
            ScorePlayer1.Text = "0";
            ScorePlayer2.Text = "0";
        }
    }
}