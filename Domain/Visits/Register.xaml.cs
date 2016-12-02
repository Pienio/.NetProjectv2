﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DatabaseAccess;
using System.Security.Cryptography;
using Visits.Validations;
using System.Security;
using DatabaseAccess.Model;
using Microsoft.Practices.Unity;
using Visits.ViewModels;
using MahApps.Metro.Controls;

namespace Visits
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : MetroWindow
    {
        [Dependency]
        public RegisterViewModel ViewModel
        {
            get { return DataContext as RegisterViewModel; }
            set { DataContext = value; }
        }
        public bool WH { get; set; }
        public Register()
        {
            InitializeComponent();
         
            

        }
    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.Initialize(WH);
        }

       

        
    }
}
