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
            mapString = @"data\maps\map1.txt";
        }
        private void Map2_Click(object sender, RoutedEventArgs e)
        {
            mapString = @"data\maps\map2.txt";
        }
        private void Map3_Click(object sender, RoutedEventArgs e)
        {
            mapString = @"data\maps\map3.txt";
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
        private void ButtonMap_MouseLeave(object sender, MouseEventArgs e)
        {
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