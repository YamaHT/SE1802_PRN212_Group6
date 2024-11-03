using Microsoft.Win32;
using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Models.Enums;
using SE1802_PRN212_Group6.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

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
        public ObservableCollection<Category> Categories { get; set; }

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
                    Temp.Name  = _select.Name;
                    OnPropertyChanged(nameof(Temp));
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
            //Types = Enum.GetValues(typeof(TypeEnum)).Cast<TypeEnum>().Select(x => x.ToString()).ToList();
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
            Products = new ObservableCollection<Product>(_unitOfWork.ProductRepository.GetAllWithDeleted());
            ImagePresentation = "Not choose";
            ImageDialog = null;
            Temp = new();
            Select = new();
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
                _unitOfWork.ProductRepository.Add(Temp);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Delete(object obj)
        {
            var get = _unitOfWork.ProductRepository.GetById(Select.Id);
            if (get != null)
            {
                _unitOfWork.ProductRepository.Remove(get);
                _unitOfWork.SaveChanges();
                Clear(obj);
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
                get.Category = Temp.Category;
  
                get.Image = ImageDialog != null
                    ? ImageUtil.UpdateImage(nameof(Product), Select.Image, ImageDialog)
                    : get.Image;

                _unitOfWork.ProductRepository.Update(get);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Restore(object obj)
        {
            var get = _unitOfWork.ProductRepository.GetById(Select.Id);
            if (get != null)
            {
                _unitOfWork.ProductRepository.Restore(get);
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
