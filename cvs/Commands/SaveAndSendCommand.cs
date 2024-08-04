using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Office.Interop.Outlook;

namespace cvs.Commands
{
    internal class SaveAndSendCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private string prepareEmailContent()
        {


            throw new NotImplementedException();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OutlookEmailer.SendEmail("cvs.svc@fakeinspection.gc.cat", "atossell91@outlook.com", "subject", "body");
            throw new NotImplementedException();
        }
    }
}
