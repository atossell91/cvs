using cvs.Models;
using cvs.ViewModels;
using Microsoft.Win32;
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
        private SaveFileDialog saveFileDialog;
        public EditorWindow()
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML File | *.xml";
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

            string timeOfDay = "morning";
            if (DateTime.Now.TimeOfDay.Hours >= 12)
            {
                timeOfDay = "afternoon";
            }

            string baseBody = "Good {0},\n\nSee attached the CVS sheet for the week of {1} (Week {2}).\n\nHave a great day,\n{3}";

            DateTime useDate = DateTime.Now;
            string nowDateStr = useDate.ToString("dddd MMMM dd, yyyy");
            string nowWeekStr = WeekNumberCalculator.CalcWeekNumber(useDate).ToString();
            string inspectorName = "Anthony";
            string body = String.Format(baseBody, timeOfDay, nowDateStr, nowWeekStr, inspectorName);

            string baseSubject = "CVS Sheet for {0} (Week {1})";
            string subject = String.Format(baseSubject, nowDateStr, nowWeekStr);

            if ((bool)saveFileDialog.ShowDialog())
            {
                string outputPath = saveFileDialog.FileName;
                File.WriteAllText(outputPath, content);
                OutlookEmailer.SendEmail("atossell91@gmail.com", "atossell91@outlook.com", subject, body, outputPath);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9:]");
        }
    }
}
