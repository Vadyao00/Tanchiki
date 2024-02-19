using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tanchiki;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Course1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string mapString = @"data\maps\map1.txt";
        public MainWindow()
        {
            InitializeComponent();
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

            using (GameScene gameScene = new(GameWindowSettings.Default, nativeWindowSettings,this,mapString))
            {
                this.Hide();
                gameScene.Run();
            }
        }
        public void ShowWindow()
        {
            this.Show();
        }

        private void Map1_Click(object sender, RoutedEventArgs e)
        {
            mapString = @"data\maps\map1.txt";
            firstMapText.Foreground = Brushes.Green;
            secondMapText.Foreground = Brushes.Black;
            thirdMapText.Foreground = Brushes.Black;
        }
        private void Map2_Click(object sender, RoutedEventArgs e)
        {
            mapString = @"data\maps\map2.txt";
            firstMapText.Foreground = Brushes.Black;
            secondMapText.Foreground = Brushes.Green;
            thirdMapText.Foreground = Brushes.Black;
        }
        private void Map3_Click(object sender, RoutedEventArgs e)
        {
            mapString = @"data\maps\map3.txt";
            firstMapText.Foreground = Brushes.Black;
            secondMapText.Foreground = Brushes.Black;
            thirdMapText.Foreground = Brushes.Green;
        }
    }
}