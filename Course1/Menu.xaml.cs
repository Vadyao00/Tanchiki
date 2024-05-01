using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Tanchiki;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Windows.Media.Effects;
using Tanchiki.UI;

namespace Course1
{
    public partial class Menu : Window
    {
        private static TextBlock ScorePlayer1 = new TextBlock();
        private static TextBlock ScorePlayer2 = new TextBlock();
        TextBlock Score;

        public Menu()
        {
            InitializeComponent();
            ScorePlayer1.Text = "0";
            ScorePlayer2.Text = "0";
            ResizeMode = ResizeMode.NoResize;
            CounterBlock.Text = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
            Score = CounterBlock;
            Score.Text = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            using (GameScene gameScene = new(GameWindowSettings.Default, nativeWindowSettings, this, ContainerClass.mapString, Score, ScorePlayer1, ScorePlayer2))
            {
                Hide();
                gameScene.Run();
            }
        }

        private void ButtonInformation_Click(object sender, RoutedEventArgs e)
        {
            InformationWindow infoWindow = new InformationWindow();
            infoWindow.Show();
        }

        private void ButtonMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            MoreInformationWindow window = new MoreInformationWindow();
            window.Show();
        }

        private void ButtonSelectMap_Click(object sender, RoutedEventArgs e)
        {
            SelectMapWindow window = new SelectMapWindow();
            window.Show();
        }

        private void ButtonResetScore_Click_1(object sender, RoutedEventArgs e)
        {
            ScorePlayer1.Text = "0";
            ScorePlayer2.Text = "0";
            Score.Text = $"Игрок 1 | {ScorePlayer1.Text} : {ScorePlayer2.Text} | Игрок 2";
        }
    }
}