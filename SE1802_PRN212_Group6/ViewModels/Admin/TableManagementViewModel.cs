using Microsoft.Win32;
using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Models.Enums;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.Admin
{
    public class TableManagementViewModel : BaseViewModel
    {
        public IReadOnlyList<string> Types { get; set; }
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

        public ObservableCollection<Table> Tables { get; set; }

        public ICommand ClearCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RestoreCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }


        private Table _select { get; set; }
        public Table Select
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
                    Temp.Floor = _select.Floor;
                    Temp.Type = _select.Type;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        private Table _temp { get; set; }
        public Table Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public TableManagementViewModel()
        {
            Types = Enum.GetValues(typeof(TypeEnum)).Cast<TypeEnum>().Select(x => x.ToString()).ToList();
            Load();

            ClearCommand = new RelayCommand(Clear);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update, (object obj) => !Select.IsDeleted);
            DeleteCommand = new RelayCommand(Delete, (object obj) => !Select.IsDeleted);
            RestoreCommand = new RelayCommand(Restore, (object obj) => Select.IsDeleted);
            ChooseImageCommand = new RelayCommand(ChooseImage);
        }

        public void Load()
        {
            Tables = new ObservableCollection<Table>(_unitOfWork.TableRepository.GetAllWithDeleted());
            ImagePresentation = "Not choose";
            ImageDialog = null;
            Temp = new();
            Select = new();
            OnPropertyChanged(nameof(Tables));
        }

        public void Add(object obj)
        {
            if (ImageDialog == null)
            {
                Dialog.ShowError("Please insert an image before add");
                return;
            }

            Temp.Image = ImageUtil.AddImage(nameof(Table), ImageDialog);
            if (Temp.TryValidate())
            {
                _unitOfWork.TableRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Dialog.ShowSuccess("Add successfully");
                Clear(obj);
            }
        }

        public void Delete(object obj)
        {
            if (Dialog.ShowConfirm($"Are you sure you want to delete this table? (Id: {Select.Id})"))
            {
                var get = _unitOfWork.TableRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.TableRepository.Remove(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Delete successfully");
                    Clear(obj);
                }
            }
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.TableRepository.GetById(Select.Id);
            if (get != null)
            {
                get.Floor = Temp.Floor;
                get.Type = Temp.Type;
                get.Image = ImageDialog != null
                    ? ImageUtil.UpdateImage(nameof(Table), Select.Image, ImageDialog)
                    : get.Image;

                if (get.TryValidate())
                {
                    _unitOfWork.TableRepository.Update(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Update successfully");
                    Clear(obj);
                }
            }
        }

        public void Restore(object obj)
        {
            var get = _unitOfWork.TableRepository.GetById(Select.Id);
            if (get != null)
            {
                _unitOfWork.TableRepository.Restore(get);
                _unitOfWork.SaveChanges();
                Dialog.ShowSuccess("Restore successfully");
                Clear(obj);
            }
        }

        public void Clear(object obj)
        {
            if (obj is ListView listRoom)
            {
                listRoom.UnselectAll();
            }
            Tables.Clear();
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
