using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Decorators;
using GroceryStore.MainApp.Models.Extensions;
using GroceryStore.MainApp.Models.PreModel;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GroceryStore.MainApp.ViewModels.PopupVM;

public partial class ProductPopupVM : PopupVMBase
{
    private readonly IDataService<ProductType> _productTypeDataService;

    private readonly int? _id = null;

    public ProductPopupVM(IPopupService dialogService, Product? product = null) : base(dialogService, product)
    {
        _productTypeDataService = App.GetService<IDataService<ProductType>>();

        LoadData();
        ProductResult = null;
        // Should loaded all resource

        // Edit
        if (product != null)
        {
            _id = product.Id;
            Name = product.Name;
            SelectedProductType = AvailibleProductType.Find(pt => ProductTypeDecorator.Equal(pt, product.Type));
            Price = product.Price;
            BasePrice = product.BasePrice;
            Quantity = product.Quantity;
        }
    }

    [ObservableProperty]
    private string _name = string.Empty;

    public List<ProductType> AvailibleProductType { get; set; } = new();
    [ObservableProperty]
    private ProductType? _selectedProductType;

    [ObservableProperty]
    private double _price = 0;

    [ObservableProperty]
    private double _basePrice = 0;

    [ObservableProperty]
    private double _quantity = 0;

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
            else if (_selectedProductType == null)
            {
                _errorMessage = "Need a Product Type";
            }
            else if (_price < 0)
            {
                _errorMessage = "Invalid price range";
            }
            else if (_price < _basePrice)
            {
                _errorMessage = "Price must be larger than base price";
            }
            else if (_basePrice < 0)
            {
                _errorMessage = "Invalid base price range";
            }
            else if (_quantity < 0)
            {
                _errorMessage = "Invalid quantity range";
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

    private void LoadData()
    {
        var productTypeDataList = Task.Run(_productTypeDataService.GetAll).Result;
        AvailibleProductType.Refresh(productTypeDataList);
    }

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

    private PMProduct? ProductResult { get; set; }
    private void RenewOrderResult()
    {
        var product = new Product()
        {
            Id = _id,
            Name = _name,
            BasePrice = _basePrice,
            Price = _price,
            Quantity = (int)_quantity,
            Type = _selectedProductType,
        };
        ProductResult = new PMProduct(product);
    }

    public override object? GetFormData()
    {
        return ProductResult;
    }
}
