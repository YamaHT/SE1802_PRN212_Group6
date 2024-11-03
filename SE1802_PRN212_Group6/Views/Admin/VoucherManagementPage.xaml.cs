using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE1802_PRN212_Group6.Views.Admin
{
    /// <summary>
    /// Interaction logic for VoucherManagementPage.xaml
    /// </summary>
    public partial class VoucherManagementPage : Page
    {
        public VoucherManagementPage()
        {
            InitializeComponent();
            datePicker.DisplayDateStart = DateTime.Now.AddDays(1);
        }
        private void NumericOnlyTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9.]+$");
        }

        private void TextOnlyTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$");
        }
        private void TextAndNumericOnlyTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z0-9.\s]+$");
        }
    }
}
