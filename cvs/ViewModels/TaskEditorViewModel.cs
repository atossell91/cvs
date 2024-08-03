using cvs.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvs.ViewModels
{
    public class TaskEditorViewModel : INotifyPropertyChanged
    {
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

        public TaskEditorViewModel(CvsSheet sheet)
        {
            this.sheet = sheet;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
