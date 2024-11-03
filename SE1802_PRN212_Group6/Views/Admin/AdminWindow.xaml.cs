using System.Windows;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
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
            MainFrame.Content = new EmployeeManagementPage();
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
    }
}
