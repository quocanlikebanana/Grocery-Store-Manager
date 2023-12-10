using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.Native;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Models.DomainExtensions;

namespace GroceryStore.MainApp.ViewModels.SubWindowVM;

public partial class OrderPopupVM : PopupVMBase
{
    private readonly IDataService<OrderDetail> _orderDeatailDataService;
    private readonly IDataService<Product> _productDataService;
    private readonly IDataService<Customer> _customerDataService;

    public OrderPopupVM(IPopupService dialogService, IDataService<OrderDetail> dataService, IDataService<Product> productDataService, IDataService<Customer> customerDataService, OrderDetail? orderDetail = null) : base(dialogService, orderDetail)
    {
        _orderDeatailDataService = dataService;
        _productDataService = productDataService;
        _customerDataService = customerDataService;
        _hasCustomer = false;
        OnLoad();

        if (orderDetail != null)
        {
            _hasCustomer = orderDetail.Order?.Customer == null ? false : true;
            _orderDate = orderDetail.Order?.OrderDate ?? DateTime.Now;
            _quantity = orderDetail.Quantity;
            _selectedCustomer = orderDetail.Order?.Customer;
            _selectedProduct = orderDetail.Product;
            _couponUsed = 0;
        }
    }

    [ObservableProperty]
    private bool _hasCustomer;

    [ObservableProperty]
    private DateTime _orderDate;

    [ObservableProperty]
    private int _quantity;

    [ObservableProperty]
    private int _couponUsed;

    [ObservableProperty]
    private Customer? _selectedCustomer;

    public ObservableCollection<Customer> AvailbleCustomer { get; private set; } = new();

    [ObservableProperty]
    private Product? _selectedProduct;

    public ObservableCollection<Product> AvailbleProduct { get; private set; } = new();

    private async void OnLoad()
    {
        AvailbleCustomer.Clear();
        var data = await _customerDataService.GetAll();
        foreach (var customer in data)
        {
            AvailbleCustomer.Add(customer);
        }

        AvailbleProduct.Clear();
        var data2 = await _productDataService.GetAll();
        foreach (var product in data2)
        {
            AvailbleProduct.Add(product);
        }
    }

    protected override bool OnAccept(object formData)
    {
        var result = Task.Run(async () => await _orderDeatailDataService.Create((OrderDetail)GetFormData()));
        if (result.Result == null)
        {
            return false;
        }
        return true;
    }

    protected override void OnInvalid() => base.OnInvalid();
    public override object GetFormData()
    {
        return new OrderDetail()
        {
            Quantity = Quantity,
            Product = SelectedProduct ?? new(),
            Order = new Order()
            {
                Id = 0,
                Customer = SelectedCustomer,
                OrderDate = OrderDate,
                TotalPrice = 0,
                //TotalPrice = Math.Min( SelectedProduct?.Price ?? 0 * Quantity - SelectedCustomer?.Coupons.Count ?? 0 * SelectedCustomer?.MoneyForPromotion ?? 0, 0),
            }
        };
    }
}

