using System.Windows;

namespace Tanchiki.UI
{
    public partial class InformationWindow : Window
    {
        public InformationWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
