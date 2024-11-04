using SE1802_PRN212_Group6.ViewModels.Login;
using SE1802_PRN212_Group6.Views;
using System.Windows;

namespace SE1802_PRN212_Group6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(MainFrame);
        }
    }
}