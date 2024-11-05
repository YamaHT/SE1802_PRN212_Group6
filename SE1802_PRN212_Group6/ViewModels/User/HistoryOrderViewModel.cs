using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.User
{
    public class HistoryOrderViewModel : BaseViewModel
    {
        public int UserId { get; set; }

        public ObservableCollection<Order> Orders { get; set; }

        public ICommand ClearCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private Order _select { get; set; }
        public Order Select
        {
            get => _select;
            set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));

                if (_select != null)
                {
                    Temp.RecipientName = _select.RecipientName;
                    Temp.Phone = _select.Phone;
                    Temp.Address = _select.Address;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        private Order _temp { get; set; }
        public Order Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public HistoryOrderViewModel(Models.User user)
        {
            UserId = user.Id;
            Load();

            ClearCommand = new RelayCommand(Clear);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Load()
        {
            Orders = new(_unitOfWork.OrderRepository.GetAllOrderedByUserId(UserId));
            Temp = new();
            Select = new();
            OnPropertyChanged(nameof(Orders));
        }

        public void Delete(object obj)
        {
            if (Dialog.ShowConfirm($"Are you sure you want to delete this order? (Id: {Select.Id})"))
            {
                var get = _unitOfWork.OrderRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.OrderRepository.Remove(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Delete successfully");
                    Clear(obj);
                }
            }
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.OrderRepository.GetById(Select.Id);
            if (get != null)
            {
                get.RecipientName = Temp.RecipientName;
                get.Phone = Temp.Phone;
                get.Address = Temp.Address;

                if (get.TryValidate())
                {
                    _unitOfWork.OrderRepository.Update(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Update successfully");
                    Clear(obj);
                }
            }
        }

        public void Clear(object obj)
        {
            if (obj is ListView listRoom)
            {
                listRoom.UnselectAll();
            }
            Orders.Clear();
            Load();
        }
    }
}
