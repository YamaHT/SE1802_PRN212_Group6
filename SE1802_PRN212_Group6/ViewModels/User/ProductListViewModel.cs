using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.User
{
    public class ProductListViewModel : BaseViewModel
    {
        public Models.User User { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        private Product _select { get; set; }
        public Product Select
        {
            get => _select; set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));
            }
        }

        public ICommand AddToCartCommand { get; }

        public ProductListViewModel(Models.User user)
        {
            this.User = user;
            Load();
            AddToCartCommand = new RelayCommand(AddToCart);
        }

        public void Load()
        {
            string[] includes = ["Category"];
            Products = new ObservableCollection<Product>(_unitOfWork.ProductRepository.GetAll(includes));
            Select = new();
            OnPropertyChanged(nameof(Products));
        }

        public void AddToCart(object obj)
        {
            if (Select.Id == 0) return;

            var order = _unitOfWork.OrderRepository.GetInCartByUserId(User.Id);
            if (order == null)
            {
                order = new Order
                {
                    User = _unitOfWork.UserRepository.GetById(User.Id),
                };
                _unitOfWork.OrderRepository.Add(order);
            }

            var orderDetail = _unitOfWork.OrderDetailRepository.GetByOrderIdAndProductId(order.Id, Select.Id);
            if (orderDetail == null)
            {
                orderDetail = new OrderDetail
                {
                    Order = order,
                    ProductId = Select.Id,
                    SubQuantity = 1,
                    SubTotal = Select.Price,
                };
                _unitOfWork.OrderDetailRepository.Add(orderDetail);
            }
            else
            {
                orderDetail.SubQuantity++;
                orderDetail.SubTotal += Select.Price;
                _unitOfWork.OrderDetailRepository.Update(orderDetail);
            }

            Dialog.ShowSuccess("Add to cart successfully");
            _unitOfWork.SaveChanges();
            Products.Clear();
            Load();
        }
    }
}
