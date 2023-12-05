using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Core.Contracts.Services;
using GroceryStore.MainApp.Core.Models;

namespace GroceryStore.MainApp.ViewModels;

public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<Order> _dataService;

    public ObservableCollection<Order> Source { get; } = new ();

    public OrderViewModel(IDataService<Order> dataService)
    {
        _dataService = dataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _dataService.GetAll();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
