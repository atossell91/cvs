﻿using cvs.Models;
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
            InitializeComponent();
        }

        public EditorWindow(TaskEditorViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9:]");
        }
    }
}
