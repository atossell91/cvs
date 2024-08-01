using cvs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvs.ViewModels
{
    public class TaskEditorViewModel
    {
        public CvsSheet sheet { get; set; }

        public TaskEditorViewModel()
        {
            sheet = CvsSheetFactory.Create("599", "K&R Poultry", DateTime.Now);
        }
    }
}
