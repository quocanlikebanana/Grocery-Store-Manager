using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Models.PreModel;
using Microsoft.UI.Xaml;
using System.Security.Cryptography;

namespace GroceryStore.MainApp.ViewModels.PopupVM
{
    public partial class CustomerPopupVM : PopupVMBase
    {
        private readonly IDataService<Coupon> _couponDS;
        private readonly int? _id = null;
        private readonly int _couponCount = 0;
        private readonly double _moneyForPromotion = -1;

        public CustomerPopupVM(IPopupService dialogService, Customer? customer = null) : base(dialogService, customer)
        {
            _couponDS = App.GetService<IDataService<Coupon>>();

            // Edit
            if (customer != null)
            {
                _id = customer.Id;
                _couponCount = customer.CouponCount;
                _moneyForPromotion = customer.MoneyForPromotion;
                Name = customer.Name;
                Tel = customer.Tel;
                Address = customer.Address;
            }
            else
            {
                _moneyForPromotion = Task.Run(async () => (await _couponDS.GetAll()).First().ThresHold).Result;
            }
        }

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _tel;

        [ObservableProperty]
        private string? _address;

        // >>>> ===========================
        // >>>> ===========================

        private string? _errorMessage = null;

        // Only update when called
        public string? ErrorMessage => _errorMessage;
        public Visibility ShouldDisplayError
        {
            get
            {
                if (_errorMessage != null || _errorMessage == string.Empty)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        private bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _errorMessage = "Can't have an empty name";
                }
                else if (string.IsNullOrEmpty(_tel))
                {
                    _errorMessage = "Must input a telephone number";
                }
                else if (string.IsNullOrEmpty(_address))
                {
                    _errorMessage = "Must input an address";
                }
                else
                {
                    _errorMessage = null;
                }
                return _errorMessage == null;
            }
        }

        // >>>> ===========================
        // >>>> ===========================

        protected override bool OnAccepting(object? formData)
        {
            if (Valid == false)
            {
                return false;
            }
            // Accepted
            RenewOrderResult();
            return true;
        }

        protected override void OnInvalid()
        {
            base.OnInvalid();
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(ShouldDisplayError));
        }

        private PMCustomer? CustomerResult { get; set; }
        private void RenewOrderResult()
        {
            var customer = new Customer()
            {
                Id = _id,
                Name = _name!,
                Tel = _tel!,
                Address = _address!,
                CouponCount = _couponCount,
                MoneyForPromotion =_moneyForPromotion,
            };
            CustomerResult = new PMCustomer(customer);
        }


        public override object? GetFormData()
        {
            return CustomerResult;
        }
    }
}
