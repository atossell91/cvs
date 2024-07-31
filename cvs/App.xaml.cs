using cvs.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        App()
        {
            MainMenu menu = new MainMenu();
            menu.ShowDialog();

            switch (menu.SelectedWindow)
            {
                case (WindowType.CreateNew):
                    TaskEditorViewModel vm = new TaskEditorViewModel();
                    vm.Display();
                    break;
                case (WindowType.LoadExisting):
                    break;
                default:
                    break;
            }

            this.Shutdown();
        }
    }
}
