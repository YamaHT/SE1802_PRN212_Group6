using System.Windows;

namespace SE1802_PRN212_Group6.Utils
{
    public static class Dialog
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool ShowConfirm(string message)
        {
            var confirm = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return confirm == MessageBoxResult.Yes;
        }
    }
}
