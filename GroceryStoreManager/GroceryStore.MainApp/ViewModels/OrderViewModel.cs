using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Views.DisplayObjects;

namespace GroceryStore.MainApp.ViewModels;


public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<OrderDetail> _orderDetailDataService;
    private readonly IPopupService _popupService;

    public OrderViewModel(IDataService<OrderDetail> orderDetailDataService)
    {
        _orderDetailDataService = orderDetailDataService;
        AddCommand = new DelegateCommand(AddRecord);
        EditCommand = new DelegateCommand(obj => { TestFunctionDialog("Edit"); });
        DeleteCommand = new DelegateCommand(obj => { TestFunctionDialog("Delete"); });

        _popupService = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Order);
    }

    public ObservableCollection<OrderDisplay> Source { get; private set; } = new();

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        var data = await _orderDetailDataService.GetAll();
        foreach (var item in data)
        {
            Source.Add(new(item));
        }
    }

    public void OnNavigatedFrom()
    {
    }

    // =======================================
    // Commands
    // =======================================

    private void TestFunctionDialog(string content)
    {
        Debug.WriteLine(content);
    }

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
}
