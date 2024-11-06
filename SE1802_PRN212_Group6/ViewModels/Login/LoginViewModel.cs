using SE1802_PRN212_Group6.Utils;
using SE1802_PRN212_Group6.Views.Admin;
using SE1802_PRN212_Group6.Views.User;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.Login
{
    public class LoginViewModel : BaseViewModel
    {
        public MainViewModel MainViewModel { get; set; }
        public string? Email { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ChangeToLoginCommand { get; }
        public ICommand ChangeToRegisterCommand { get; }


        public LoginViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            ChangeToLoginCommand = new RelayCommand(ChangeToLogin);
            ChangeToRegisterCommand = new RelayCommand(ChangeToRegister);
        }

        public void Login(object obj)
        {
            var passwordBox = obj as PasswordBox;

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(passwordBox?.Password))
            {
                Dialog.ShowError("Please fulfill email and password");
                return;
            }

            var validateUser = new Models.User { Email = Email, Password = passwordBox.Password };
            if (!validateUser.TryValidate())
            {
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
                new AdminWindow(user).Show();
            }
            else
            {
                new UserWindow(user).Show();
            }

            Application.Current.MainWindow.Visibility = Visibility.Hidden;
        }

        public void Register(object obj)
        {
            var passwordBox = obj as PasswordBox;

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(passwordBox?.Password))
            {
                Dialog.ShowError("Please fulfill email and password");
                return;
            }

            if (_unitOfWork.UserRepository.CheckEmailExisted(Email))
            {
                Dialog.ShowError("This email is existed");
                return;
            }

            var user = new Models.User
            {
                Email = Email,
                Password = passwordBox.Password,
            };

            if (user.TryValidate())
            {
                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.SaveChanges();
                Dialog.ShowSuccess("Register successfully");
                ChangeToLogin(obj);
            }
        }

        public void ChangeToLogin(object obj)
        {
            MainViewModel.NavigateToLoginPage();
        }

        public void ChangeToRegister(object obj)
        {
            MainViewModel.NavigateToRegisterPage();
        }
    }
}
