using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Models.Extensions;
using GroceryStore.MainApp.Models.PreModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GroceryStore.MainApp.ViewModels;

public partial class ProductViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<Product> _productDataService;

    public IPopupService _popupService;

    public ProductViewModel()
    {
        _productDataService = App.GetService<IDataService<Product>>();
        _popupService = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Product);

        AddCommand = new DelegateCommand(AddRecord);
        EditCommand = new DelegateCommand(EditRecord);
        DeleteCommand = new DelegateCommand(DeleteRecord);
        DetailCommand = new DelegateCommand(DetailRecord);

        ReloadCommand = new DelegateCommand(Reload);
        ClearCriteriaCommand = new DelegateCommand(ClearCriteria);
        PagingCommand = new DelegateCommand(Paging);
    }

    public ObservableCollection<Product> Source { get; private set; } = new();

    [ObservableProperty]
    private Product? _selectedProduct = null;

    public async void OnNavigatedTo(object parameter)
    {
        // Loading
        await LoadData();
    }

    public void OnNavigatedFrom()
    {
    }

    // =======================================
    // View
    // =======================================    

    private async Task LoadData()
    {
        var lowerLimit = _thousandUnit ? _lowerLimit * 1000 : _lowerLimit;
        var upperLimit = _thousandUnit ? _upperLimit * 1000 : _upperLimit;
        var data = await _productDataService.GetFull(_searchText, _selectedSortColumn, _asc, lowerLimit, upperLimit, (int)_perPage, _pageNum);
        Source.Refresh(data.Items);
        TotalPage = data.TotalPage;
        OnPropertyChanged(nameof(Source));
        OnPropertyChanged(nameof(TotalPage));
    }

    public Dictionary<string, string> SortColumns = new()
    {
        { "Name", "Name" },
        { "Type", "TypeId" },
        { "Price", "Price" },
        { "Base price", "BasePrice" },
        { "Quantity", "Quantity" },
    };

    [ObservableProperty]
    private string _selectedSortColumn = "";

    [ObservableProperty]
    private bool _asc = true;

    [ObservableProperty]
    private double? _lowerLimit = null;

    [ObservableProperty]
    private double? _upperLimit = null;

    [ObservableProperty]
    private string _searchText = "";

    [ObservableProperty]
    private int _pageNum = 1;

    [ObservableProperty]
    private int _totalPage = 0;

    // Must reload the page because of paging (and many other things)
    public ICommand ReloadCommand { get; }
    public ICommand ClearCriteriaCommand { get; }
    public ICommand PagingCommand { get; }

    private async void Reload(object? param)
    {
        PageNum = 1;
        await LoadData();
    }

    private void ClearCriteria(object? param)
    {
        if (param is string criteria)
        {
            if (criteria == "Sort")
            {
                SelectedSortColumn = "";
                return;
            }
            if (criteria == "Filter")
            {
                UpperLimit = null;
                LowerLimit = null;
                return;
            }
            if (criteria == "Search")
            {
                SearchText = "";
                return;
            }
        }
    }

    private async void Paging(object? param)
    {
        if (param is string strDir && int.TryParse(strDir, out int dir) == true)
        {
            if (dir == -2)
            {
                PageNum = 1;
            }
            else if (dir == -1)
            {
                PageNum = Math.Max(PageNum - 1, 1);
            }
            else if (dir == 1)
            {
                PageNum = Math.Min(PageNum + 1, TotalPage);
            }
            else if (dir == 2)
            {
                PageNum = TotalPage;
            }
            else
            {
                return;
            }
            await LoadData();
        }
    }

    // =======================================
    // CRUD
    // =======================================

    public ICommand AddCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand DetailCommand { get; private set; }

    //public ICommand QuickEditQuantityCommand { get; }

    private async void AddRecord(object? obj)
    {
        var result = await _popupService.ShowWindow();
        if (result != null && result is PMProduct pmProduct)
        {
            // TODO: Display loading screen here
            var insertResult = await pmProduct.Insert();
            if (insertResult == true)
            {
                Reload(null);
                return;
            }
        }
        // Display system error
    }

    private async void EditRecord(object? obj)
    {
        if (SelectedProduct == null)
        {
            return;
        }
        var result = await _popupService.ShowWindow(SelectedProduct);
        if (result != null && result is PMProduct pmProduct)
        {
            // TODO: Display loading screen here
            var updateResult = await pmProduct.Update();
            if (updateResult == true)
            {
                Reload(null);
                return;
            }
        }
        // Display system error
    }

    private async void DeleteRecord(object? obj)
    {
        if (SelectedProduct == null)
        {
            return;
        }
        var pmOrder = new PMProduct(SelectedProduct);
        // Loading
        var checkCascade = await pmOrder.CheckCascade();
        if (checkCascade == true)
        {
            var warning = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Warning);
            var message = "This record is already been used by some Orders. Delete all related Orders?";
            var acceptCascade = await warning.ShowWindow(message);
            // a little bit dirty here, null means decline
            if (acceptCascade is null)
            {
                return;
            }
        }
        var deleteResult = await pmOrder.RawDelete();
        if (deleteResult == true)
        {
            Reload(null);
            return;
        }
        // Error
    }

    public Action<int>? GoToDetail { get; set; }

    private void DetailRecord(object? obj)
    {
        if (SelectedProduct == null || SelectedProduct.Id is null)
        {
            return;
        }
        GoToDetail?.Invoke((int)SelectedProduct.Id!);
    }

    // =======================================
    // Settings
    // =======================================

    [ObservableProperty]
    private double _perPage = 10;
    partial void OnPerPageChanged(double value)
    {
        Reload(null);
    }

    [ObservableProperty]
    private bool _allowAddEdit = true;

    [ObservableProperty]
    private bool _allowDelete = true;

    [ObservableProperty]
    private bool _allowQuickEdit = true;

    [ObservableProperty]
    private bool _thousandUnit = true;
}
