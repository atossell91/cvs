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

namespace cvs.ViewModels
{
    /// <summary>
    /// Interaction logic for DateSelector.xaml
    /// </summary>
    public partial class DateSelector : Window
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public DateSelector()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
