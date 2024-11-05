using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models.Enums;
using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.ViewModels.User
{
    public class Table_BookingViewModel : BaseViewModel
    {
        public ObservableCollection<Booking> Bookings { get; set; }
        public IReadOnlyList<Table> Tables { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public List<string> TimeBox { get; set; } = ["18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00"];

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
                    Temp.Note = _select.FullName;
                    Temp.BookingDate = _select.BookingDate;
                    Temp.ArrivalTime = _select.ArrivalTime;
                    Temp.NumberOfPeople = _select.NumberOfPeople;
                    Temp.Table = _select.Table;

                    OnPropertyChanged(nameof(Temp.Table));
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

        public Table_BookingViewModel()
        {
            Tables = _unitOfWork.TableRepository.GetAll();

            Load();

            ClearCommand = new RelayCommand(Clear);
            AddCommand = new RelayCommand(Add);
        }

        public void Load()
        {
            string[] includes = ["Table"];
            Bookings = new ObservableCollection<Booking>(_unitOfWork.BookingRepository.GetAllWithDeleted(includes));
            Temp = new();
            Select = new();
            OnPropertyChanged(nameof(Bookings));
        }

        public void Add(object obj)
        {

            if (Temp.TryValidate())
            {
                Temp.Table = _unitOfWork.TableRepository.GetById(Temp.Table.Id);
                _unitOfWork.BookingRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Delete(object obj)
        {
            var get = _unitOfWork.BookingRepository.GetById(Select.Id);
            if (get != null)
            {
                _unitOfWork.BookingRepository.Remove(get);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.BookingRepository.GetById(Select.Id);
            if (get != null)
            {
                get.FullName = Temp.FullName;
                get.Note = Temp.Note;
                get.Phone = Temp.Phone;

                _unitOfWork.BookingRepository.Update(get);
                _unitOfWork.SaveChanges();
                Clear(obj);
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

