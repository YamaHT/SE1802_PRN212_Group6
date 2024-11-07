using SE1802_PRN212_Group6.Models;
using SE1802_PRN212_Group6.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Input;

namespace SE1802_PRN212_Group6.ViewModels.User
{
    public class CheckoutInfoDTO
    {
        [Required(ErrorMessage = "Recipient Name is required")]
        public string? RecipientName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string? Phone { get; set; }

        public Voucher? Voucher { get; set; }
    }

    public class CartViewModel : BaseViewModel
    {
        public Models.User User { get; set; }
        public Order? Order { get; set; }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }
        public IReadOnlyList<Voucher> Vouchers { get; set; }

        public CheckoutInfoDTO CheckoutInfoDTO { get; set; }

        private OrderDetail _select { get; set; }
        public OrderDetail Select
        {
            get => _select; set
            {
                _select = value;
                OnPropertyChanged(nameof(Select));

                if (_select != null)
                {
                    Temp.SubQuantity = _select.SubQuantity;
                    OnPropertyChanged(nameof(Temp));
                }
            }
        }

        private OrderDetail _temp { get; set; }
        public OrderDetail Temp
        {
            get => _temp; set
            {
                _temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        public ICommand MinusCommand { get; set; }
        public ICommand PlusCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }

        public CartViewModel(Models.User user)
        {
            this.User = user;
            Vouchers = _unitOfWork.VoucherRepository.GetAllValidVouchers();
            CheckoutInfoDTO = new();
            Load();

            MinusCommand = new RelayCommand(Minus, (object obj) => Select != null && Select.ProductId != 0);
            PlusCommand = new RelayCommand(Plus, (object obj) => Select != null && Select.ProductId != 0);
            ClearCommand = new RelayCommand(Clear);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            CheckoutCommand = new RelayCommand(Checkout);
        }

        public void Load()
        {
            Order = _unitOfWork.OrderRepository.GetInCartByUserId(User.Id);
            if (Order == null)
            {
                Order = new() { User = _unitOfWork.UserRepository.GetById(User.Id) };
                _unitOfWork.OrderRepository.Add(Order);
                _unitOfWork.SaveChanges();
            }

            OrderDetails = new(Order.OrderDetails);
            Temp = new();
            Select = new();
            OnPropertyChanged(nameof(Order));
            OnPropertyChanged(nameof(OrderDetails));
        }

        public void Minus(object obj)
        {
            Temp.SubQuantity = Math.Max(1, Temp.SubQuantity - 1);
            OnPropertyChanged(nameof(Temp));
        }

        public void Plus(object obj)
        {
            Temp.SubQuantity++;
            OnPropertyChanged(nameof(Temp));
        }

        public void Update(object obj)
        {
            var get = _unitOfWork.OrderDetailRepository.GetByOrderIdAndProductId(Select.OrderId, Select.ProductId);
            if (get != null)
            {
                get.SubQuantity = Temp.SubQuantity;
                get.SubTotal = Temp.SubQuantity * get.Product!.Price;
                _unitOfWork.OrderDetailRepository.Update(get);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Delete(object obj)
        {
            var get = _unitOfWork.OrderDetailRepository.GetByOrderIdAndProductId(Select.OrderId, Select.ProductId);
            if (get != null)
            {
                _unitOfWork.OrderDetailRepository.Remove(get);
                _unitOfWork.SaveChanges();
                Clear(obj);
            }
        }

        public void Checkout(object obj)
        {
            if (Order!.OrderDetails.Count == 0)
            {
                Dialog.ShowError("No products in cart to checkout");
                return;
            }

            if (!CheckoutInfoDTO.TryValidate())
            {
                return;
            }

            Order.Quantity = Order.OrderDetails.Sum(x => x.SubQuantity);
            Order.Total = Order.OrderDetails.Sum(x => x.SubTotal);

            var actualPayment = Order.Total;

            if (CheckoutInfoDTO.Voucher != null)
            {
                var voucher = _unitOfWork.VoucherRepository.GetById(CheckoutInfoDTO.Voucher.Id)!;

                actualPayment = Order.Total - Math.Min(Order.Total * (voucher.ReducedPercent / 100), voucher.MaxReducing);

                voucher.Quantity = Math.Max(0, voucher.Quantity - 1);
                _unitOfWork.VoucherRepository.Update(voucher);

                Order.Voucher = voucher;
            }

            Order.ActualPayment = actualPayment;
            Order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            Order.RecipientName = CheckoutInfoDTO.RecipientName;
            Order.Address = CheckoutInfoDTO.Address;
            Order.Phone = CheckoutInfoDTO.Phone;

            if (Order.TryValidate())
            {
                Dialog.ShowSuccess("Checkout successfully");

                _unitOfWork.OrderRepository.Update(Order);
                _unitOfWork.SaveChanges();

                CheckoutInfoDTO = new();
                Vouchers = _unitOfWork.VoucherRepository.GetAllValidVouchers();

                OnPropertyChanged(nameof(CheckoutInfoDTO));
                OnPropertyChanged(nameof(Vouchers));

                Clear(obj);
            }
        }

        public void Clear(object obj)
        {
            if (obj is ListView listRoom)
            {
                listRoom.UnselectAll();
            }
            Load();
        }
    }
}
