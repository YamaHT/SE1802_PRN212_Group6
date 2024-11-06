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
        public Models.User User { get; set; }
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<string> FilteredTimeBox { get; set; }
        public List<string> TimeBox { get; set; }
        public IReadOnlyList<Table> Tables { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        private Table _select { get; set; }
        public Table Select
        {
            get => _select;
            set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));
                FilterAvailableTimes(); 
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
                FilterAvailableTimes();

            }
        }

        public Table_BookingViewModel(Models.User user)
        {
            User = user;
            Tables = _unitOfWork.TableRepository.GetAll();
            TimeBox = new List<string> { "18:00", "18:30", "19:00", "19:30", "20:00", "20:30", "21:00" };
            FilteredTimeBox = new ObservableCollection<string>(TimeBox);    
            Load();
            ClearCommand = new RelayCommand(Clear);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update, (object obj) => !Select.IsDeleted);

        }


        public void FilterAvailableTimes()
        {
            // Kiểm tra rằng `Temp.BookingDate` và `Select` có giá trị hợp lệ
            if (Temp.BookingDate == default || Select == null) return;

            // Xóa danh sách giờ trống hiện tại
            FilteredTimeBox.Clear();

            // Lọc các booking có cùng Table Id và BookingDate với các giờ đã đặt
            var existingBookings = Bookings
                .Where(b => b.Table.Id == Select.Id && b.BookingDate == Temp.BookingDate)
                .Select(b => b.ArrivalTime.ToString("HH:mm")); // Chuyển TimeOnly thành string

            // Duyệt qua từng khung giờ trong TimeBox và chỉ thêm các giờ trống vào FilteredTimeBox
            foreach (var time in TimeBox)
            {
                if (!existingBookings.Contains(time))
                {
                    FilteredTimeBox.Add(time);
                }
            }

            // Thông báo cập nhật UI
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
                FilterAvailableTimes();
                Dialog.ShowSuccess("Booking successfully");
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

