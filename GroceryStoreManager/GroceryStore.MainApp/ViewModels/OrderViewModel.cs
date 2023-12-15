using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Models.Extensions;

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
        _popupService.OnPopupAcceptSucess += PopupDataSubmit;
    }

    private void PopupDataSubmit(object obj)
    {
        var order = (Order)obj;
        Source.Add(order);
    }

    public ObservableCollection<Order> Source { get; private set; } = new();

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

    public ICommand AddCommand
    {
        get; private set;
    }
    public ICommand EditCommand
    {
        get; private set;
    }
    public ICommand DeleteCommand
    {
        get; private set;
    }
    private void AddRecord(object? obj)
    {
        _popupService.ShowWindow();
    }
    private void EditRecord(object? obj)
    {
        _popupService.ShowWindow();
    }
    private void DeleteRecord(object? obj)
    {
        _popupService.ShowWindow();
    }
}
