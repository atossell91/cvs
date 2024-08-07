using cvs.Commands;
using cvs.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cvs.ViewModels
{
    public class TaskEditorViewModel : INotifyPropertyChanged
    {
        private SaveFileDialog xmlSaverDialog;
        public ICommand saveSend { get; private set; }

        public CvsSheet sheet { get; set; }

        public int WeekNum { 
            get
            {
                return sheet.WeekNumber;
            }
            set
            {
                sheet.WeekNumber = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("WeekNumber"));
            }
        }

        public TaskEditorViewModel()
        {
            xmlSaverDialog = new SaveFileDialog();
            xmlSaverDialog.Filter = "XML File | *.xml";
            saveSend = new SaveAndSendCommand(xmlSaverDialog);
        }

        public TaskEditorViewModel(CvsSheet sheet) : this()
        {
            this.sheet = sheet;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
