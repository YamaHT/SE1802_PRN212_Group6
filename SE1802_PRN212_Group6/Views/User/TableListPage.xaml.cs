using SE1802_PRN212_Group6.ViewModels.User;
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
    /// Interaction logic for TableListPage.xaml
    /// </summary>
    public partial class TableListPage : Page
    {
        public TableListPage(Models.User user)
        {
            InitializeComponent();
            DataContext = new Table_BookingViewModel(user);
        }
        private void bookingDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            bookingDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddDays(-1)));
        }

    }
}
