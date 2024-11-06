using System.Windows;
using System.Windows.Controls;
using SE1802_PRN212_Group6.Models; 
namespace SE1802_PRN212_Group6.Views.Admin
{
    public partial class AdminWindow : Window
    {
        private readonly Models.User User;

        public AdminWindow(Models.User user)
        {
            InitializeComponent();
            User = user; 
        }

        private void MenuItemToCheck(MenuItem itemToCheck)
        {
            foreach (var removeItem in Menu.Items)
            {
                (removeItem as MenuItem).IsChecked = false;
            }
            itemToCheck.IsChecked = true;
        }

        private void EmployeeM_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EmployeeManagementPage(User);
            MenuItemToCheck(EmployeeM);
        }

        private void ProductM_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProductManagementPage();
            MenuItemToCheck(ProductM);
        }

        private void TableM_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new TableManagementPage();
            MenuItemToCheck(TableM);
        }

        private void VoucherM_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new VoucherManagementPage();
            MenuItemToCheck(VoucherM);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.MainWindow.Visibility = Visibility.Visible;
        }
    }
}
