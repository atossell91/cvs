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

namespace cvs
{
    public enum WindowType
    {
        CreateNew,
        LoadExisting,
        None
    }

    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public WindowType SelectedWindow { get; set; } = WindowType.None;
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedWindow = WindowType.CreateNew;
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectedWindow = WindowType.None;
            this.Hide();
        }
    }
}
