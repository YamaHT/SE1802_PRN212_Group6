using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.User
{
    public class Table_BookingViewModel : BaseViewModel
    {
        public Models.User User { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<string> FilteredTimeBox { get; set; }
        public List<string> TimeBox { get; set; }
        public IReadOnlyList<Table> Tables { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand OnDateChangeCommand { get; set; }

        private Table _select { get; set; }
        public Table Select
        {
            get => _select;
            set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));
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

        public Table_BookingViewModel(Models.User user)
        {
            User = user;
            Tables = _unitOfWork.TableRepository.GetAll();
            TimeBox = ["18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00"];
            FilteredTimeBox = [];
            Load();
            AddCommand = new RelayCommand(Add);
            OnDateChangeCommand = new RelayCommand((object obj) => FilterAvailableTimes());
        }

        public void FilterAvailableTimes()
        {
            if (Select == null || Select?.Id == 0)
            {
                FilteredTimeBox.Clear();
                return;
            }

            FilteredTimeBox.Clear();

            var existingBookings = Bookings
                .Where(b => b.Table.Id == Select.Id && b.BookingDate == Temp.BookingDate)
                .Select(b => b.ArrivalTime?.ToString("HH:mm"));

            foreach (var time in TimeBox)
            {
                if (!existingBookings.Contains(time))
                {
                    FilteredTimeBox.Add(time);
                }
            }

            OnPropertyChanged(nameof(FilteredTimeBox));
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
            Temp.User = _unitOfWork.UserRepository.GetById(User.Id);
            Temp.Table = _unitOfWork.TableRepository.GetById(Select.Id);
            if (Temp.TryValidate())
            {
                _unitOfWork.BookingRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Clear(obj);
                Dialog.ShowSuccess("Booking successfully");
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

