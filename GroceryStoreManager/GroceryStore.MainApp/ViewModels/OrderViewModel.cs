using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Data.Extensions;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Decorators;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Models.Extensions;
using GroceryStore.MainApp.Models.PreModel;

namespace GroceryStore.MainApp.ViewModels;


public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<Order> _orderDataService;
    public IPopupService _popupService;

    public OrderViewModel(IDataService<Order> orderDataService)
    {
        _orderDataService = orderDataService;
        AddCommand = new DelegateCommand(AddRecord);
        EditCommand = new DelegateCommand(EditRecord);
        DeleteCommand = new DelegateCommand(DeleteRecord);

        _popupService = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Order);
        //_popupService.OnPopupAcceptSucess += PopupDataSubmit;
    }

    //private void PopupDataSubmit(object? obj)
    //{
    //    if (obj is PMOrder pmOrder)
    //    {
    //        var insertResult = Task.Run(pmOrder.Insert).Result;
    //        if (insertResult == true)
    //        {
    //            // Display system error
    //            var order = pmOrder.GetFullObject();
    //            Source.Add(order);
    //        }
    //    }
    //}

    public ObservableCollection<Order> Source { get; private set; } = new();
    [ObservableProperty]
    private Order? _selectedOrder = null;

    public async void OnNavigatedTo(object parameter)
    {
        var data = await _orderDataService.GetAll();
        Source.Refresh(data);
    }

    public void OnNavigatedFrom()
    {
    }

    // =======================================
    // Commands
    // =======================================

    public ICommand AddCommand { get; private set; }
    public ICommand EditCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    private async void AddRecord(object? obj)
    {
        var result = await _popupService.ShowWindow();
        if (result != null && result is PMOrder pmOrder)
        {
            // TODO: Display loading screen here
            var insertResult = await pmOrder.Insert();
            if (insertResult == true)
            {
                var order = pmOrder.GetFullObject();
                Source.Add(order);
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
                var order = pmOrder.GetFullObject();
                var index = Source.FindIndex(x => OrderDecorator.Equal(x, order));
                Source[index] = order;
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
            Source.Remove(SelectedOrder);
            return;
        }
        // Error
    }
}
