using System.Windows;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.Views.User
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private Models.User User;

        public UserWindow(Models.User user)
        {
            InitializeComponent();
            this.User = user;
        }

        private void MenuItemToCheck(MenuItem itemToCheck)
        {
            foreach (var removeItem in Menu.Items)
            {
                (removeItem as MenuItem).IsChecked = false;
            }
            itemToCheck.IsChecked = true;
        }

        private void ProductList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProductListPage();
            MenuItemToCheck(ProductList);
        }

        private void TableList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new TableListPage();
            MenuItemToCheck(TableList);
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CartPage();
            MenuItemToCheck(Cart);
        }

        private void HistoryOrder_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HistoryOrderPage();
            MenuItemToCheck(HistoryOrder);
        }

        private void HistoryBooking_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HistoryBookingPage();
            MenuItemToCheck(HistoryBooking);
        }
    }
}
