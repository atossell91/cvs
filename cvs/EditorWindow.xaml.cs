using cvs.Models;
using cvs.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public EditorWindow()
        {
            InitializeComponent();
        }

        public EditorWindow(TaskEditorViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskEditorViewModel viewModel = (TaskEditorViewModel)DataContext;
            string text = CvsSheetXmlSerializer.Serialize(viewModel.sheet);
            TaskEditorViewModel vm = (TaskEditorViewModel)this.DataContext;
            CvsSheet sheet = vm.sheet;
            string content = CvsSheetXmlSerializer.Serialize(sheet);
            File.WriteAllText("C:\\Users\\atoss\\Programming\\comparer\\output.xml", content);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9:]");
        }
    }
}
