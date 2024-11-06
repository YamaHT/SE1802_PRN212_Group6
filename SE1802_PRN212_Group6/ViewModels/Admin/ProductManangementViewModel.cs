using Microsoft.Win32;
using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.Admin
{
    public class ProductManangementViewModel : BaseViewModel
    {
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

        public ObservableCollection<Product> Products { get; set; }
        public IReadOnlyList<Category> Categories { get; set; }

        public ICommand ClearCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RestoreCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }


        private Product _select { get; set; }
        public Product Select
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
                    Temp.Price = _select.Price;
                    Temp.Description = _select.Description;
                    Temp.Category = _select.Category;

                    OnPropertyChanged(nameof(Temp));
                    OnPropertyChanged(nameof(Temp.Category));
                }
            }
        }

        private Product _temp { get; set; }
        public Product Temp
        {
            get => _temp;
            set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public ProductManangementViewModel()
        {
            Categories = _unitOfWork.CategoryRepository.GetAll();
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
            string[] includes = ["Category"];
            Products = new ObservableCollection<Product>(_unitOfWork.ProductRepository.GetAllWithDeleted(includes));
            Temp = new();
            Select = new();
            ImagePresentation = "Not choose";
            ImageDialog = null;
            OnPropertyChanged(nameof(Products));
        }

        public void Add(object obj)
        {
            if (ImageDialog == null)
            {
                Dialog.ShowError("Please insert an image before add");
                return;
            }

            Temp.Image = ImageUtil.AddImage(nameof(Product), ImageDialog);
            if (Temp.TryValidate())
            {
                Temp.Category = _unitOfWork.CategoryRepository.GetById(Temp.Category.Id);
                _unitOfWork.ProductRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Dialog.ShowSuccess("Added successfully");
                Clear(obj);
            }
        }

        public void Delete(object obj)
        {
            if (Dialog.ShowConfirm($"Are you sure you want to delete this product? (Id: {Select.Id})"))
            {

                var get = _unitOfWork.ProductRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.ProductRepository.Remove(get);
                    _unitOfWork.SaveChanges();
                    Clear(obj);
                }
            }
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.ProductRepository.GetById(Select.Id);
            if (get != null)
            {
                get.Name = Temp.Name;
                get.Price = Temp.Price;
                get.Description = Temp.Description;
                get.Category = _unitOfWork.CategoryRepository.GetById(Temp.Category.Id);

                get.Image = ImageDialog != null
                    ? ImageUtil.UpdateImage(nameof(Product), Select.Image, ImageDialog)
                    : get.Image;
                if (get.TryValidate())
                {
                    _unitOfWork.ProductRepository.Update(get);
                    _unitOfWork.SaveChanges();
                    Dialog.ShowSuccess("Updated successfully");
                    Clear(obj);
                }
            }
        }

        public void Restore(object obj)
        {
            if (Dialog.ShowConfirm($"Are you sure you want to restore this product? (Id: {Select.Id})"))
            {
                var get = _unitOfWork.ProductRepository.GetById(Select.Id);
                if (get != null)
                {
                    _unitOfWork.ProductRepository.Restore(get);
                    _unitOfWork.SaveChanges();
                    Clear(obj);
                }
            }
        }

        public void Clear(object obj)
        {
            if (obj is ListView listProduct)
            {
                listProduct.UnselectAll();
            }
            Products.Clear();
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
