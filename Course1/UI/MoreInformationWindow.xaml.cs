using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tanchiki.UI
{
    /// <summary>
    /// Логика взаимодействия для MoreInformationWindow.xaml
    /// </summary>
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
