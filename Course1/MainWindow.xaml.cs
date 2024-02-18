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
        public MainWindow()
        {
            InitializeComponent();
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var nativeWindowSettings = new NativeWindowSettings
            {
                WindowState = OpenTK.Windowing.Common.WindowState.Maximized,
                WindowBorder = WindowBorder.Resizable,
                StartFocused = true,
                StartVisible = true,
                Flags = ContextFlags.Default,
                APIVersion = new Version(3, 3),
                Profile = ContextProfile.Compatability,
                API = ContextAPI.OpenGL,
            };

            using (GameScene gameScene = new(GameWindowSettings.Default, nativeWindowSettings,this))
            {
                this.Hide();
                gameScene.Run();
            }
        }
        public void ShowWindow()
        {
            this.Show();
        }
    }
}