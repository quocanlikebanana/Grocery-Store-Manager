using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Data.Extensions;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Decorators;
using GroceryStore.MainApp.Models.Extensions;
using GroceryStore.MainApp.Models.PreModel;
using Microsoft.UI.Xaml;

namespace GroceryStore.MainApp.ViewModels.SubWindowVM;

public partial class OrderPopupVM : PopupVMBase
{
    private readonly IDataService<Order> _orderDataService;
    private readonly IDataService<Product> _productDataService;
    private readonly IDataService<Customer> _customerDataService;
    private readonly IDataService<Coupon> _couponDataService;

    private int? _id = null;

    public OrderPopupVM(IPopupService dialogService, IDataService<Order> orderDataService, IDataService<Product> productDataService, IDataService<Customer> customerDataService, IDataService<Coupon> couponDataService, Order? order = null) : base(dialogService, order)
    {
        _orderDataService = orderDataService;
        _productDataService = productDataService;
        _customerDataService = customerDataService;
        _couponDataService = couponDataService;

        LoadData();
        OrderResult = null;
        CustomerSection = true;
        // Should loaded all resource

        // Edit
        if (order != null)
        {
            _id = order.Id;
            OrderDate = order.OrderDate;
            SelectedCustomer = AvailibleCustomer.Find(cus => CustomerDecorator.Equal(cus, order.Customer));
            TotalPrice = order.TotalPrice;
            Details.Refresh(order.details);
            CustomerSection = false;
            _baseTotalDiscount = order.TotalDiscount;
        }
        AddToOrderCommand = new DelegateCommand(AddToOrder);
        DeleteFromOrderCommand = new DelegateCommand(DeleteFromOrder);
    }

    // >>>> Customer
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CouponCustomerHas), nameof(HasCustomer))]
    private Customer? _selectedCustomer;
    public List<Customer> AvailibleCustomer { get; } = new();
    // <<<< Customer

    // >>>> Sub section: Order Details
    // >>>>>>>> Product
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MaxQuantity), nameof(HasProduct))]
    private Product? _selectedProduct;
    public List<Product> AvailbleProduct { get; } = new();
    // <<<<<<<< Product
    [ObservableProperty]
    private double _quantity = 1;

    public double MaxQuantity => _selectedProduct?.Quantity ?? 1;
    public bool HasProduct => SelectedProduct is not null;
    public ICommand AddToOrderCommand
    {
        get;
    }
    public ICommand DeleteFromOrderCommand
    {
        get;
    }
    public ObservableCollection<OrderDetail> Details { get; private set; } = new();
    private void AddToOrder(object? param)
    {
        if (SelectedProduct == null || Quantity < 1 || Quantity > MaxQuantity)
        {
            return;
        }
        var orderDetail = new OrderDetail()
        {
            ProductId = SelectedProduct.Id ?? -1,
            Product = SelectedProduct,
            Quantity = (int)Quantity,
        };
        Details.Add(orderDetail);
        // trigger
        TotalPrice += Quantity * SelectedProduct.Price;
        SelectedProduct.Quantity -= (int)Quantity;
        OnPropertyChanged(nameof(MaxQuantity));
    }
    private void DeleteFromOrder(object? param)
    {
        var odParam = (OrderDetail)param!;
        // For the edit case (when previous product is not in the list)
        var productInList = AvailbleProduct.Find(x => x.Id == odParam.ProductId);
        Details.Remove(odParam);
        // trigger
        TotalPrice -= odParam.Quantity * odParam.Product!.Price;
        productInList!.Quantity += odParam.Quantity;
        OnPropertyChanged(nameof(MaxQuantity));
    }
    // <<<< Sub section: Order Details

    // <<<< Date
    [ObservableProperty]
    private DateTime _orderDate = DateTime.Now;
    public DateTime DateTimeNow => DateTime.Now;
    // >>>> Date

    // >>>> Coupon
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalDiscount))]
    private double _couponUsed;
    private double temCouponUsed = 0;
    partial void OnCouponUsedChanging(double value)
    {
        temCouponUsed = (int)value - (int)_couponUsed;
    }
    partial void OnCouponUsedChanged(double value)
    {
        TotalPrice -= temCouponUsed * CurrentCouponValue;
    }

    public bool HasCustomer => SelectedCustomer is not null && CustomerSection;
    public double CouponCustomerHas => SelectedCustomer?.CouponCount ?? 0;
    private double _currentCouponValue = -9999;
    public double CurrentCouponValue => _currentCouponValue;
    private double _baseTotalDiscount = 0;
    public double TotalDiscount
    {
        get
        {
            if (CustomerSection)
            {
                return CouponUsed * CurrentCouponValue;
            }
            return _baseTotalDiscount;
        }
    }
    // <<<< Coupon

    private double _temBorrow = 0;
    [ObservableProperty]
    private double _totalPrice;
    partial void OnTotalPriceChanged(double value)
    {
        if (CustomerSection == true)
        {
            if (value < 0)
            {

                if (CouponUsed > 0)
                {
                    CouponUsed--;
                }
                else
                {
                    // System error
                }
            }
        }
        // Edit mode
        else
        {
            if (value >= 0)
            {
                _totalPrice += _temBorrow;
                _temBorrow = 0;
            }
            else
            {
                _totalPrice = 0;
                _temBorrow += value;
            }
        }
    }


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
            if (TotalPrice < 0)
            {
                _errorMessage = "Negative total price";
                return false;
            }
            if (SelectedCustomer is null)
            {
                _errorMessage = "Customer not yet selected";
                return false;
            }
            if (CouponUsed < 0 || CouponUsed > CouponCustomerHas)
            {
                _errorMessage = "Invalid coupon amount";
                return false;
            }
            if (Details.Count == 0)
            {
                _errorMessage = "Order has no product";
                return false;
            }
            var testTotalPrice = 0.0;
            foreach (var item in Details)
            {
                testTotalPrice += (item.Product?.Price ?? 0) * item.Quantity;
            }
            testTotalPrice -= TotalDiscount;
            if (testTotalPrice != TotalPrice)
            {
                // TODO: hide this, because its system error
                _errorMessage = "Total price is incorrect";
                return false;
            }
            _errorMessage = null;
            return true;
        }
    }

    // >>>> ===========================
    // >>>> ===========================

    public bool CustomerSection { get; private set; }

    // >>>> ===========================
    // >>>> ===========================

    private void LoadData()
    {
        _currentCouponValue = Task.Run(async () => (await _couponDataService.GetAll()).First().perCoupon).Result;
        var customerData = Task.Run(_customerDataService.GetAll).Result;
        AvailibleCustomer.Refresh(customerData);
        var productData = Task.Run(_productDataService.GetAll).Result;
        AvailbleProduct.Refresh(productData);
    }

    protected override bool OnAccepting(object? formData)
    {
        if (Valid == false)
        {
            return false;
        }
        // Accepted
        RenewOrderResult();
        SelectedCustomer!.CouponCount -= (int)CouponUsed;
        return true;
    }

    protected override void OnInvalid()
    {
        base.OnInvalid();
        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(ShouldDisplayError));
    }

    private PMOrder? OrderResult { get; set; }
    private void RenewOrderResult()
    {
        var order = new Order()
        {
            Id = _id,
            Customer = SelectedCustomer,
            CustomerID = SelectedCustomer?.Id,
            details = Details.ToList(),
            OrderDate = OrderDate,
            TotalDiscount = TotalDiscount,
            TotalPrice = TotalPrice,
        };
        OrderResult = new PMOrder(order, (int)CouponUsed);
    }

    public override object? GetFormData()
    {
        return OrderResult;
    }
}



