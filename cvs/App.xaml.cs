using cvs.Models;
using cvs.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace cvs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public CvsSheet LoadExistingSheet()
        {
            CvsSheet sheet = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml|All Files|*.*";
            if((bool)ofd.ShowDialog())
            {
                sheet = CvsSheetXmlSerializer.Deserialize(ofd.FileName);
            }

            return sheet;
        }

        public CvsSheet CreateNewSheet()
        {
            DateSelector dateSelector = new DateSelector();
            dateSelector.ShowDialog();

            CvsSheet sheet = CvsSheetFactory.Create("599", "K&R Poultry", dateSelector.Date);

            return sheet;
        }
        App()
        {

            CvsSheet sheet = null;

            MainMenu menu = new MainMenu();
            menu.ShowDialog();

            switch(menu.SelectedWindow)
            {
                case WindowType.LoadExisting:
                    sheet = LoadExistingSheet();
                    break;
                case WindowType.CreateNew:
                    sheet = CreateNewSheet();
                    break;
            }

            if (sheet != null)
            {
                TaskEditorViewModel taskEditorViewModel = new TaskEditorViewModel(sheet);
                EditorWindow editorWindow = new EditorWindow(taskEditorViewModel);
                editorWindow.ShowDialog();
            }

            this.Shutdown();
        }
    }
}
