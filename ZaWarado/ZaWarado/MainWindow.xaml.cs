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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZaWarado.Models;

namespace ZaWarado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Style ButtonStyle = new Style();
        public MainWindow()
        {
            InitializeComponent();
            SetUpStyle();
            DisplayHand();
            
        }

        private void DisplayHand()
        {
            int handSize = 5;
            for (int i = 0; i < handSize; i++)
            {
                Button newBtn = new Button();
                newBtn.Name = "Card" + i.ToString();
                newBtn.MaxHeight = 200;
                newBtn.MaxWidth = 120;
                newBtn.MinHeight = 60;
                newBtn.MinWidth = 120;

                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("../../Assets/Images/Plant.png", UriKind.Relative));
                newBtn.Background = brush;
                Hand.Children.Add(newBtn);
            }
        }
        private void SetUpBoard()
        {

        }
        private void SetUpStyle()
        {
            ButtonStyle.TargetType = typeof(Button);
            ButtonStyle.Setters.Add(new Setter(new DependencyProperty(OverridesDefaultStyle), true));
        }
        //Set button background code

        //var brush = new ImageBrush();
        //brush.ImageSource = new BitmapImage(new Uri("Images/ContentImage.png", UriKind.Relative));
        //button1.Background = brush;


    }
}
