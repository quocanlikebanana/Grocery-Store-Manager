using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Core.Contracts.Services;
using GroceryStore.MainApp.Core.Models;
using GroceryStore.MainApp.Services;
using GroceryStore.MainApp.ViewModels.SubWindowVM;
using GroceryStore.MainApp.Views;
using GroceryStore.MainApp.Views.SubWindowView;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace GroceryStore.MainApp.ViewModels;


public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<OrderDetail> _orderDetailDataService;

    public OrderViewModel(IDataService<OrderDetail> orderDetailDataService)
    {
        _orderDetailDataService = orderDetailDataService;

        AddCommand = new DelegateCommand(obj => { TestFunctionDialog("Add"); });
        EditCommand = new DelegateCommand(obj => { TestFunctionDialog("Edit"); });
        DeleteCommand = new DelegateCommand(obj => { TestFunctionDialog("Delete"); });
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

    // No DI here cause all of these should only belong to this VM
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
        var _orderDataService = App.GetService<IDataService<Order>>();
        var _productDataService = App.GetService<IDataService<Product>>();
        var _customerDataService = App.GetService<IDataService<Customer>>();
        IWindowDialogService dialogService = new FormDialogService(typeof(OrderForm), (wds) => new OrderFormVM(wds, _orderDetailDataService, _productDataService, _customerDataService, null));
        dialogService.ShowWindow();

    }

}
