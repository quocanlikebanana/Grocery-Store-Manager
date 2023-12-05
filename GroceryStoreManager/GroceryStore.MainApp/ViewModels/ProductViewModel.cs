using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Core.Contracts.Services;
using GroceryStore.MainApp.Core.Models;

namespace GroceryStore.MainApp.ViewModels;

public partial class ProductViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<Product> _dataService;

    [ObservableProperty]
    private Product? selected;

    public ObservableCollection<Product> Source { get; private set; } = new ();

    public ProductViewModel(IDataService<Product> dataService)
    {
        _dataService = dataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        var data = await _dataService.GetAll();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected ??= Source.First();
    }
}
