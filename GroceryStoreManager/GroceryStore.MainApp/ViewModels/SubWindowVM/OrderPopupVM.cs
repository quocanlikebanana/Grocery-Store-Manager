using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Models.Extensions;
using GroceryStore.MainApp.Models.PreModel;

namespace GroceryStore.MainApp.ViewModels.SubWindowVM;

public partial class OrderPopupVM : PopupVMBase
{
    private readonly IDataService<Order> _orderDeatailDataService;
    private readonly IDataService<Product> _productDataService;
    private readonly IDataService<Customer> _customerDataService;
    private readonly IDataService<Coupon> _couponDataService;
    private readonly PMOrder _order;

    public OrderPopupVM(IPopupService dialogService, IDataService<Order> dataService, IDataService<Product> productDataService, IDataService<Customer> customerDataService, IDataService<Coupon> couponDataService, Order? order = null) : base(dialogService, order)
    {
        _orderDeatailDataService = dataService;
        _productDataService = productDataService;
        _customerDataService = customerDataService;
        _couponDataService = couponDataService;

        _order = new PMOrder(order);
        _orderDate = _order.OrderDate;
        _selectedCustomer = _order.Customer;
        _couponUsed = _order.CouponUsed;
        _totalPrice = _order.TotalPrice;
        OnLoad();
        Details.Refresh(_order.Details);

        AddToOrderCommand = new DelegateCommand(AddToOrder);
        DeleteFromOrderCommand = new DelegateCommand(DeleteFromOrder);

        //DeclineCommand = new DelegateCommand(Decline);
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
    private Product? _selectedProduct;
    public List<Product> AvailbleProduct { get; } = new();
    // <<<<<<<< Product
    [ObservableProperty]
    private double _quantity = 1;
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
        if (SelectedProduct == null || Quantity < 1)
        {
            return;
        }
        var orderDetail = new OrderDetail()
        {
            Product = SelectedProduct,
            Quantity = (int)Quantity,
        };
        Details.Add(orderDetail);
        TotalPrice += Quantity * SelectedProduct.Price;
    }
    private void DeleteFromOrder(object? param)
    {
        var orderDetail = (OrderDetail)param!;
        Details.Remove(orderDetail);
        TotalPrice -= orderDetail.Quantity * orderDetail.Product!.Price;
    }
    // <<<< Sub section: Order Details

    // <<<< Date
    [ObservableProperty]
    private DateTime _orderDate;
    // >>>> Date

    // >>>> Coupon
    [ObservableProperty]
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

    public bool HasCustomer => SelectedCustomer is not null;
    public double CouponCustomerHas => SelectedCustomer?.CouponCount ?? 0;
    public double CurrentCouponValue
    {
        get
        {
            var queryRes = Task.Run(async () => (await _couponDataService.GetAll()).First().perCoupon).Result;
            return queryRes;
        }
    }
    // <<<< Coupon

    public DateTime DateTimeNow => DateTime.Now;
    [ObservableProperty]
    private double _totalPrice;
    partial void OnTotalPriceChanged(double value)
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

    //public ICommand AcceptCommand => new DelegateCommand(Accept);
    //public ICommand DeclineCommand { get; }

    // >>>> ===========================
    // >>>> ===========================

    private bool valid
    {
        get
        {
            var res = true;
            if (TotalPrice < 0)
            {
                res = false;
            }
            else if (SelectedCustomer is null)
            {
                res = false;
            }
            else if (Details.Count == 0)
            {
                res = false;
            }
            return res;
        }
    }

    // >>>> ===========================
    // >>>> ===========================

    private void OnLoad()
    {
        var customerData = Task.Run(_customerDataService.GetAll).Result;
        AvailibleCustomer.Refresh(customerData);
        var productData = Task.Run(_productDataService.GetAll).Result;
        AvailbleProduct.Refresh(productData);
    }

    protected override bool AcceptResultCheck(object formData)
    {
        if (valid == false)
        {
            return false;
        }
        // Bug o day 
        var result = Task.Run(async () => await _orderDeatailDataService.Create((Order)GetFormData()));
        if (result.Result == null)
        {
            return false;
        }
        return true;
    }

    public override object GetFormData()
    {
        var pmOrder = new PMOrder()
        {
            OrderDate = OrderDate,
            CouponUsed = (int)CouponUsed,
            DiscountPerCoupon = CurrentCouponValue,
            Customer = SelectedCustomer,
            Details = Details.ToList(),
        };
        return pmOrder.Get();
    }
}



