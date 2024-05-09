using System.Windows;

namespace Tanchiki.UI
{
    public partial class MoreInformationWindow : Window
    {
        public MoreInformationWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            InfoTextBlock.Text = "В игровом окне слева сверху отображено три информационных шкалы для первого игрока, справа сверху - для второго игрока.\nКрасная шкала отвечает за количество жизней, фиолетовая - за количество топлива, оранжевая - за количество снарядов.";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
