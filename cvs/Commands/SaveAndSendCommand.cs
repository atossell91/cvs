using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using cvs.Services;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Win32;

namespace cvs.Commands
{
    internal class SaveAndSendCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string InspectorName { get; set; }
        public DateTime SheetDate { get; set; }
        public int SheetWeekNumber { get; set; }
        public string XmlContent { get; set; }

        private SaveFileDialog saveFileDialog;

        public SaveAndSendCommand(SaveFileDialog saveDialog)
        {
            saveFileDialog = saveDialog;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //  Save the file
            if (!(bool)saveFileDialog.ShowDialog()) return;
            string filename = saveFileDialog.FileName;
            File.WriteAllText(filename, XmlContent);

            //  Send the file
            string subject = EmailPreparer.PrepareSubjectText(SheetDate, SheetWeekNumber);
            string body = EmailPreparer.PrepareBodyText(SheetDate, SheetWeekNumber, InspectorName);
            OutlookEmailer.SendEmail(ToEmailAddress, FromEmailAddress, subject, body, filename);
        }
    }
}
