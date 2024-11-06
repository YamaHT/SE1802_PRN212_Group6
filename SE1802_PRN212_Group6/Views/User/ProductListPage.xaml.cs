using SE1802_PRN212_Group6.ViewModels.User;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.Views.User
{
    /// <summary>
    /// Interaction logic for ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public ProductListPage(Models.User user)
        {
            InitializeComponent();
            DataContext = new ProductListViewModel(user);
        }
    }
}
