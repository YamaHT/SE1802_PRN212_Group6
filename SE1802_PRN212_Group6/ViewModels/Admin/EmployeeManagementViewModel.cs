using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SE1802_PRN212_Group6.Models.Enums;
using SE1802_PRN212_Group6.Utils;
using System.Windows.Input;
using SE1802_PRN212_Group6.Models;
using Microsoft.Win32;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.ViewModels.Admin
{
    public class EmployeeManagementViewModel : BaseViewModel
    {
        public Models.User User { get; set; }
        private OpenFileDialog? _imageDialog { get; set; }
        public OpenFileDialog? ImageDialog
        {
            get => _imageDialog; set
            {
                _imageDialog = value;
                OnPropertyChanged(nameof(ImageDialog));
            }
        }

        private string _imagePresentation { get; set; }
        public string? ImagePresentation
        {
            get => _imagePresentation; set
            {
                _imagePresentation = value;
                OnPropertyChanged(nameof(ImagePresentation));
            }
        }

        public ObservableCollection<Employee> Employees { get; set; }

        public ICommand ClearCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }


        private Employee _select { get; set; }
        public Employee Select
        {
            get => _select;
            set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));

                if (_select != null)
                {
                    ImagePresentation = _select.Image;
                    Temp.ManagerId = _select.ManagerId;
                    Temp.Image = _select.Image;
                    ImageDialog = null;
                    Temp.Salary = _select.Salary;
                    Temp.Phone = _select.Phone;
                    Temp.Birthday = _select.Birthday;
                    Temp.FullName = _select.FullName;
                    Temp.Gender = _select.Gender;
                    Temp.IdentificationCard = _select.IdentificationCard;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        private Employee _temp { get; set; }
        public Employee Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public EmployeeManagementViewModel(Models.User user)
        {
            User = user ; 
            ClearCommand = new RelayCommand(Clear);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            ChooseImageCommand = new RelayCommand(ChooseImage);
            Load();
        }

        public void Load()
        {
            Employees = new ObservableCollection<Employee>(
                _unitOfWork.EmployeeRepository.GetAll(["User"]).Where(e => e.ManagerId == User.Id)
            );
            Temp = new Employee();
            Select = new Employee();
            ImagePresentation = "Not chosen";
            ImageDialog = null;
            OnPropertyChanged(nameof(Employees));
        }

        public void Add(object obj)
        {
            if (ImageDialog == null)
            {
                Dialog.ShowError("Please insert an image before adding.");
                return;
            }

            var validationError = ValidateEmployeeData();
            if (validationError != null)
            {
                Dialog.ShowError(validationError);
                return;
            }
            Temp.ManagerId = User.Id;
            Temp.IdentificationCard = Temp.IdentificationCard.Trim();
            Temp.Phone = Temp.Phone.Trim();

            Temp.Image = ImageUtil.AddImage(nameof(Employee), ImageDialog);
            if (Temp.TryValidate())
            {
                _unitOfWork.EmployeeRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Clear(obj);
                Dialog.ShowSuccess("Add complete");
            }
        }



        public void Delete(object obj)
        {
            var get = _unitOfWork.EmployeeRepository.GetById(Select.Id);
            if (get != null)
            {
                Boolean ct = Dialog.ShowConfirm("Delete employee ??");
                if (ct)
                {
                    _unitOfWork.EmployeeRepository.Remove(get);
                    _unitOfWork.SaveChanges();
                }
                Clear(obj);
            }
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.EmployeeRepository.GetById(Select.Id);

            if (get != null)
            {
                var hasIdCardChanged = get.IdentificationCard != Temp.IdentificationCard.Trim();
                var hasPhoneChanged = get.Phone != Temp.Phone.Trim();
                var hasBirthdayChange = get.Birthday != Temp.Birthday;

                if (hasIdCardChanged)
                {
                    var validationError = ValidateIdentificationCard();
                    if (validationError != null)
                    {
                        Dialog.ShowError(validationError);
                        return;
                    }
                }

                if (hasPhoneChanged)
                {
                    var validationError = ValidatePhone();
                    if (validationError != null)
                    {
                        Dialog.ShowError(validationError);
                        return;
                    }
                }

                if (hasBirthdayChange)
                {
                    var validationError = ValidateBirthday();
                    if (validationError != null)
                    {
                        Dialog.ShowError(validationError);
                        return;
                    }
                }

                if (Temp.TryValidate())
                {
                    get.ManagerId = Temp.ManagerId;
                    get.Salary = Temp.Salary;
                    get.Phone = Temp.Phone.Trim();
                    get.Birthday = Temp.Birthday;
                    get.Gender = Temp.Gender;
                    get.IdentificationCard = Temp.IdentificationCard.Trim();
                    get.FullName = Temp.FullName;
                    get.Image = ImageDialog != null
                        ? ImageUtil.UpdateImage(nameof(Employee), Select.Image, ImageDialog)
                        : get.Image;

                    _unitOfWork.EmployeeRepository.Update(get);
                    _unitOfWork.SaveChanges();
                    Clear(obj);
                    Dialog.ShowSuccess("Update complete");
                }

            }
        }




        public void Clear(object obj)
        {
            if (obj is ListView listRoom)
            {
                listRoom.UnselectAll();
            }
            Employees.Clear();
            Load();
        }

        public void ChooseImage(object obj)
        {
            ImageDialog = ImageUtil.GetImageDialog();
            if (ImageDialog != null)
            {
                ImagePresentation = ImageDialog.SafeFileName;
            }
        }

        private string ValidateEmployeeData()
        {
            string validationError;

            validationError = ValidateIdentificationCard();
            if (validationError != null) return validationError;

            validationError = ValidatePhone();
            if (validationError != null) return validationError;

            validationError = ValidateBirthday();
            if (validationError != null) return validationError;

            return null;
        }

        private string ValidateIdentificationCard()
        {
            var trimmedIdCard = Temp.IdentificationCard.Trim();

            if (trimmedIdCard != Temp.IdentificationCard || trimmedIdCard.Contains(" "))
            {
                return "IdentificationCard cannot have leading, trailing, or internal spaces.";
            }

            if (_unitOfWork.EmployeeRepository.GetAll()
                .Any(e => e.IdentificationCard == trimmedIdCard && e.Id != Temp.Id))
            {
                return "IdentificationCard already exists. Please use a different one.";
            }

            return null;
        }

        private string ValidatePhone()
        {
            var trimmedPhone = Temp.Phone.Trim();
            if (trimmedPhone != Temp.Phone || trimmedPhone.Contains(" "))
            {
                return "Phone cannot have leading, trailing, or internal spaces. ";
            }
            if (_unitOfWork.EmployeeRepository.GetAll()
                .Any(e => e.Phone == trimmedPhone && e.Id != Temp.Id))
            {
                return "Phone number already exists. Please use a different one.";
            }

            return null;
        }

        private string ValidateBirthday()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var yesterday = today.AddDays(-1);

            if (Temp.Birthday > yesterday)
            {
                return "Birthday cannot be later than yesterday.";
            }

            var age = today.Year - Temp.Birthday.Year;
            if (Temp.Birthday > today.AddYears(-age)) age--;

            if (age < 18 || age > 70)
            {
                return "Age must be between 18 and 70 years.";
            }

            return null;
        }



    }
}
