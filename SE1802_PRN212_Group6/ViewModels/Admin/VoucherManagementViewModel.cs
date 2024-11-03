using Microsoft.Win32;
using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.Admin
{
    public class VoucherManagementViewModel : BaseViewModel
    {
        private OpenFileDialog? _imageDialog { get; set; }
        public OpenFileDialog? ImageDialog
        {
            get => _imageDialog;
            set
            {
                _imageDialog = value;
                OnPropertyChanged(nameof(ImageDialog));
            }
        }

        private string _imagePresentation { get; set; }
        public string? ImagePresentation
        {
            get => _imagePresentation;
            set
            {
                _imagePresentation = value;
                OnPropertyChanged(nameof(ImagePresentation));
            }
        }

        public ObservableCollection<Voucher> Vouchers { get; set; }

        public ICommand ClearCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RestoreCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }

        private DateTime _expiredDatePicker;
        public DateTime ExpiredDatePicker
        {
            get => _temp.ExpiredDate.ToDateTime(new TimeOnly(0, 0));
            set
            {
                _expiredDatePicker = value;
                Temp.ExpiredDate = DateOnly.FromDateTime(_expiredDatePicker);
                OnPropertyChanged(nameof(ExpiredDatePicker));
            }
        }

        private Voucher _select { get; set; }
        public Voucher Select
        {
            get => _select;
            set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));

                if (_select != null)
                {
                    ImagePresentation = _select.Image;
                    ImageDialog = null;
                    Temp.Image = _select.Image;
                    Temp.Name = _select.Name;
                    Temp.ReducedPercent = _select.ReducedPercent;
                    Temp.MaxReducing = _select.MaxReducing;
                    Temp.Quantity = _select.Quantity;
                    Temp.Description = _select.Description;
                    Temp.ExpiredDate = _select.ExpiredDate;
                    ExpiredDatePicker = Temp.ExpiredDate.ToDateTime(new TimeOnly(0, 0));
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        private Voucher _temp { get; set; }
        public Voucher Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public VoucherManagementViewModel()
        {
            Load();
            ClearCommand = new RelayCommand(Clear);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update, obj => Select?.Id > 0);
            DeleteCommand = new RelayCommand(Delete, obj => Select?.Id > 0 && !Select.IsDeleted);
            RestoreCommand = new RelayCommand(Restore, obj => Select.IsDeleted);
            ChooseImageCommand = new RelayCommand(ChooseImage);
        }

        public void Load()
        {
            Vouchers = new ObservableCollection<Voucher>(_unitOfWork.VoucherRepository.GetAllAndRemoveExpired());
            _unitOfWork.SaveChanges();
            ImagePresentation = "Not choose";
            ImageDialog = null;
            Temp = new();
            Select = new();
            ExpiredDatePicker = DateTime.Now;
            OnPropertyChanged(nameof(Vouchers));
        }

        public void Add(object obj)
        {
            if (ImageDialog == null)
            {
                Dialog.ShowError("Please insert an image before adding.");
                return;
            }

            if (Temp.ExpiredDate < DateOnly.FromDateTime(DateTime.Now))
            {
                Dialog.ShowError("The expiration date cannot be set after today.");
                return;
            }

            Temp.Image = ImageUtil.AddImage(nameof(Voucher), ImageDialog);

            if (Temp.TryValidate())
            {
                _unitOfWork.VoucherRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Delete(object obj)
        {
            if (Dialog.ShowConfirm("Are you sure you want to delete this voucher?"))
            {
                var get = _unitOfWork.VoucherRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.VoucherRepository.Remove(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Voucher deleted successfully.");
                    Clear(obj);
                }
            }
        }

        public void Update(object obj)
        {
            if (Temp.ExpiredDate < DateOnly.FromDateTime(DateTime.Now))
            {
                Dialog.ShowError("The expiration date cannot be set after today.");
                return;
            }

            if (Temp.TryValidate())
            {
                var get = _unitOfWork.VoucherRepository.GetById(Select.Id);
                if (get != null)
                {
                    get.Name = Temp.Name;
                    get.ReducedPercent = Temp.ReducedPercent;
                    get.MaxReducing = Temp.MaxReducing;
                    get.Quantity = Temp.Quantity;
                    get.Description = Temp.Description;
                    get.ExpiredDate = Temp.ExpiredDate;
                    get.Image = ImageDialog != null
                        ? ImageUtil.UpdateImage(nameof(Voucher), Select.Image, ImageDialog)
                        : get.Image;
                    _unitOfWork.VoucherRepository.Update(get);
                    _unitOfWork.SaveChanges();
                    Clear(obj);
                }
            }
        }

        public void Restore(object obj)
        {
            if (Select.ExpiredDate < DateOnly.FromDateTime(DateTime.Now))
            {
                Dialog.ShowError("Cannot restore expired vouchers.");
                return;
            }

            if (Dialog.ShowConfirm("Are you sure you want to restore this voucher?"))
            {
                var get = _unitOfWork.VoucherRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.VoucherRepository.Restore(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Voucher restored successfully.");
                    Clear(obj);
                }
            }
        }

        public void Clear(object obj)
        {
            if (obj is ListView listView)
            {
                listView.UnselectAll();
            }
            Vouchers.Clear();
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
    }
}
