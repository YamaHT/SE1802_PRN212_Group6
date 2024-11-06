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
using SE1802_PRN212_Group6.ViewModels.Admin;

namespace SE1802_PRN212_Group6.Views.Admin
{
    /// <summary>
    /// Interaction logic for EmployeeManagementPage.xaml
    /// </summary>
    public partial class EmployeeManagementPage : Page
    {
        private Models.User User;
        public EmployeeManagementPage(Models.User user)
        {
            InitializeComponent();
            this.User = user;
            this.DataContext = new EmployeeManagementViewModel(user);
        }
    }
}
