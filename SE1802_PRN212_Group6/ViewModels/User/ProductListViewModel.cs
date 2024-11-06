using SE1802_PRN212_Group6.Data;
using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Controls;

namespace SE1802_PRN212_Group6.ViewModels.User
{
    public class ProductListViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; set; }

        public ProductListViewModel()
        {
            Load();
        }

        public void Load()
        {
            string[] includes = ["Category"];
            Products = new ObservableCollection<Product>(_unitOfWork.ProductRepository.GetAll(includes));

            OnPropertyChanged(nameof(Products));
        }
    }
}
