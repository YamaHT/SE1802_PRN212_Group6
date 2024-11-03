using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Utils;
using SE1802_PRN212_Group6.Views.Admin;
using SE1802_PRN212_Group6.Views.User;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string? Email { get; set; }
        public ICommand LoginCommand { get; }

        public LoginViewModel() : base()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public void Login(object obj)
        {
            var passwordBox = obj as PasswordBox;

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(passwordBox?.Password))
            {
                Dialog.ShowError("Please fulfill email and password");
                return;
            }

            var user = _unitOfWork.UserRepository.GetByEmailAndPassword(Email, passwordBox.Password);
            if (user == null)
            {
                Dialog.ShowError("Invalid email or password");
                return;
            }

            if (user.Role)
            {
                new AdminWindow().Show();
            }
            else
            {
                new UserWindow().Show();
            }

            App.Current.MainWindow.Close();
        }
    }
}
