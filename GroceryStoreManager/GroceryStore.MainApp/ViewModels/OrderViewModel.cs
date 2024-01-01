using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Helpers;
using GroceryStore.MainApp.Models.Extensions;
using GroceryStore.MainApp.Models.PreModel;

namespace GroceryStore.MainApp.ViewModels;


public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<Order> _orderDataService;
    public IPopupService _popupService;

    public OrderViewModel()
    {
        _orderDataService = App.GetService<IDataService<Order>>();
        _popupService = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Order);

        AddCommand = new DelegateCommand(AddRecord);
        EditCommand = new DelegateCommand(EditRecord);
        DeleteCommand = new DelegateCommand(DeleteRecord);
        DetailCommand = new DelegateCommand(DetailRecord);

        ReloadCommand = new DelegateCommand(Reload);
        ClearCriteriaCommand = new DelegateCommand(ClearCriteria);
        PagingCommand = new DelegateCommand(Paging);
    }

    public ObservableCollection<Order> Source { get; private set; } = new()
    {
    };

    [ObservableProperty]
    private Order? _selectedOrder = null;

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
        var data = await _orderDataService.GetFull(_searchText, _selectedSortColumn, _asc, _lowerLimit, _upperLimit, (int)_perPage, _pageNum);
        Source.Refresh(data.Items);
        TotalPage = data.TotalPage;
        OnPropertyChanged(nameof(Source));
        OnPropertyChanged(nameof(TotalPage));
    }

    public Dictionary<string, string> SortColumns = new()
    {
        { "Customer", "CustomerID" },
        { "Order date", "OrderDate" },
        { "Total price", "TotalPrice" },
        { "Total discount", "TotalDiscount" },
    };

    [ObservableProperty]
    private string _selectedSortColumn = "";

    [ObservableProperty]
    private bool _asc = true;

    [ObservableProperty]
    private DateTime? _lowerLimit = null;

    [ObservableProperty]
    private DateTime? _upperLimit = null;

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

    private async void AddRecord(object? obj)
    {
        var result = await _popupService.ShowWindow();
        if (result != null && result is PMOrder pmOrder)
        {
            // TODO: Display loading screen here
            var insertResult = await pmOrder.Insert();
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
        if (SelectedOrder == null)
        {
            return;
        }
        var result = await _popupService.ShowWindow(SelectedOrder);
        if (result != null && result is PMOrder pmOrder)
        {
            // TODO: Display loading screen here
            var updateResult = await pmOrder.Update();
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
        if (SelectedOrder == null)
        {
            return;
        }
        var pmOrder = new PMOrder(SelectedOrder);
        // Loading
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
        if (SelectedOrder == null || SelectedOrder.Id is null)
        {
            return;
        }
        GoToDetail?.Invoke((int)SelectedOrder.Id!);
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
}
