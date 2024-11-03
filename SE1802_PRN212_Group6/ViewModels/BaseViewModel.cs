using SE1802_PRN212_Group6.Data;
using System.ComponentModel;

namespace SE1802_PRN212_Group6.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly UnitOfWork _unitOfWork;

        protected BaseViewModel()
        {
            _unitOfWork = new UnitOfWork();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
