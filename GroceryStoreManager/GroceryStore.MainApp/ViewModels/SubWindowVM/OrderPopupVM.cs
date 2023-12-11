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

    public OrderPopupVM(IPopupService dialogService, IDataService<Order> dataService, IDataService<Product> productDataService, IDataService<Customer> customerDataService, Order? order = null) : base(dialogService, order)
    {
        _orderDeatailDataService = dataService;
        _productDataService = productDataService;
        _customerDataService = customerDataService;

        var pmOrder = new PMOrder(order);
        _orderDate = pmOrder.OrderDate ?? DateTime.Now;
        _selectedCustomer = pmOrder.Customer;
        Details.Refresh(pmOrder.Details);
        _couponUsed = pmOrder.CouponUsed;
        _discountPerCoupon = pmOrder.DiscountPerCoupon;
        _totalPrice = pmOrder.TotalPrice;
        OnLoad();

        AddToOrderCommand = new DelegateCommand(AddToOrder);
        DeleteFromOrderCommand = new DelegateCommand(DeleteFromOrder);
    }

    [ObservableProperty]
    private Customer? _selectedCustomer;
    public ObservableCollection<Customer> AvailbleCustomer { get; private set; } = new();

    // >>>> Sub section: Order Details
    public ObservableCollection<OrderDetail> Details { get; private set; } = new();
    [ObservableProperty]
    private Product? _selectedProduct;
    public ObservableCollection<Product> AvailbleProduct { get; private set; } = new();
    public int Quantity { get; private set; } = 0;

    public ICommand AddToOrderCommand
    {
        get;
    }
    private void AddToOrder(object? param)
    {
        var orderDetail = new OrderDetail()
        {
        };
        Details.Add(orderDetail);
    }
    public ICommand DeleteFromOrderCommand
    {
        get;
    }
    private void DeleteFromOrder(object? param)
    {
        if (param != null)
        {
            var orderDetail = (OrderDetail)param;
            Details.Remove(orderDetail);
        }
        throw new Exception();
    }
    // <<<< Sub section: Order Details

    [ObservableProperty]
    private DateTime _orderDate;

    [ObservableProperty]
    private int _couponUsed;

    // >>>> Unchangeable
    // Change this by going to the Discount setting
    [ObservableProperty]
    private double _discountPerCoupon;

    [ObservableProperty]
    private double _totalPrice;
    // <<<< Unchangeable

    private async void OnLoad()
    {
        var cusomterData = await _customerDataService.GetAll();
        AvailbleCustomer.Refresh(cusomterData);
        var productData = await _productDataService.GetAll();
        AvailbleProduct.Refresh(productData);
    }

    protected override bool ContinueAccept(object formData)
    {
        var result = Task.Run(async () => await _orderDeatailDataService.Create((Order)GetFormData()));
        if (result.Result == null)
        {
            return false;
        }
        return true;
    }

    protected override void OnInvalid()
    {
        // Action on invalid
    }

    public override object GetFormData()
    {
        var pmOrder = new PMOrder()
        {
            OrderDate = OrderDate,
            CouponUsed = CouponUsed,
            DiscountPerCoupon = DiscountPerCoupon,
            Customer = SelectedCustomer,
            Details = Details.ToList(),
        };
        return pmOrder.GetOrder();
    }
}



