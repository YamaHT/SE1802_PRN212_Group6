using SE1802_PRN212_Group6.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.ViewModels.Login
{
    public class MainViewModel
    {
        private readonly Frame _mainFrame;

        public MainViewModel(Frame mainFrame)
        {
            _mainFrame = mainFrame;
            NavigateToLoginPage();
        }

        public void NavigateToLoginPage()
        {
            _mainFrame.Content = new LoginPage { DataContext = new LoginViewModel(this) };
        }

        public void NavigateToRegisterPage()
        {
            _mainFrame.Content = new RegisterPage { DataContext = new LoginViewModel(this) };
        }
    }

}
