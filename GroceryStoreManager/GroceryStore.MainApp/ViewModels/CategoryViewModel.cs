using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Core.Contracts.Services;
using GroceryStore.MainApp.Core.Models;

namespace GroceryStore.MainApp.ViewModels;

public partial class CategoryViewModel : ObservableRecipient, INavigationAware
{
    private readonly IDataService<ProductType> _dataService;

    public ObservableCollection<ProductType> Source { get; } = new();

    public CategoryViewModel(IDataService<ProductType> dataService)
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
}
