using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Tanchiki.UI
{
    public partial class SelectMapWindow : Window
    {
        private bool IsMap1Clicked = false;
        private bool IsMap2Clicked = false;
        private bool IsMap3Clicked = false;

        public SelectMapWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Map1_Click(object sender, RoutedEventArgs e)
        {
            IsMap1Clicked = true;
            IsMap3Clicked = false;
            IsMap2Clicked = false;
            ContainerClass.mapString = @"data\maps\map1.txt";
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
            this.Close();
        }
        private void Map2_Click(object sender, RoutedEventArgs e)
        {
            IsMap2Clicked = true;
            IsMap1Clicked = false;
            IsMap3Clicked = false;
            ContainerClass.mapString = @"data\maps\map2.txt";
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
            this.Close();
        }
        private void Map3_Click(object sender, RoutedEventArgs e)
        {
            IsMap3Clicked = true;
            IsMap1Clicked = false;
            IsMap2Clicked = false;
            ContainerClass.mapString = @"data\maps\map3.txt";
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
            this.Close();
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
    }
}
