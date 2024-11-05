using SE1802_PRN212_Group6.ViewModels.User;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.Views.User
{
    /// <summary>
    /// Interaction logic for HistoryOrderPage.xaml
    /// </summary>
    public partial class HistoryOrderPage : Page
    {
        public HistoryOrderPage(Models.User user)
        {
            InitializeComponent();
            DataContext = new HistoryOrderViewModel(user);
        }
    }
}
