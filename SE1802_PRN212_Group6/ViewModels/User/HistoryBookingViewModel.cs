using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Models.Enums;
using SE1802_PRN212_Group6.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.User
{
   public class HistoryBookingViewModel : BaseViewModel
    {
        public ObservableCollection<Booking> Bookings { get; set; }

        public ICommand ClearCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private Booking _select { get; set; }
        public Booking Select
        {
            get => _select;
            set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));

                if (_select != null)
                {
                    Temp.FullName = _select.FullName;
                    Temp.Phone = _select.Phone;
                    Temp.Note = _select.Note;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        private Booking _temp { get; set; }
        public Booking Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public HistoryBookingViewModel()
        {
            Load();

            ClearCommand = new RelayCommand(Clear);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Load()
        {
            Bookings = new ObservableCollection<Booking>(_unitOfWork.BookingRepository.GetAllWithDeleted());
            Temp = new();
            Select = new();
            OnPropertyChanged(nameof(Bookings));
        }
   
        public void Delete(object obj)
        {
            if (Dialog.ShowConfirm($"Are you sure you want to delete this booking? (Id: {Select.Id})"))
            {
                var get = _unitOfWork.BookingRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.BookingRepository.Remove(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Delete successfully");
                    Clear(obj);
                }
            }
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.BookingRepository.GetById(Select.Id);
            if (get != null)
            {
                get.FullName = Temp.FullName;
                get.Phone = Temp.Phone;
                get.Note = Temp.Note;

                if (get.TryValidate())
                {
                    _unitOfWork.BookingRepository.Update(get);
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
            Bookings.Clear();
            Load();
        }    
    }
}

