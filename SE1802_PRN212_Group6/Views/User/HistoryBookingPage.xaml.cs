﻿using SE1802_PRN212_Group6.ViewModels.User;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE1802_PRN212_Group6.Views.User
{
    /// <summary>
    /// Interaction logic for HistoryBookingPage.xaml
    /// </summary>
    public partial class HistoryBookingPage : Page
    {
        public HistoryBookingPage(Models.User user)
        {
            InitializeComponent();
        }
    }
}
