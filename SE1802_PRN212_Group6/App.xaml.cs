using SE1802_PRN212_Group6.Utils;
using System.Windows;

namespace SE1802_PRN212_Group6
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("Error: {0}", e.Exception.Message);
            Dialog.ShowError(errorMessage);
            e.Handled = true;
        }
    }
}
