using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Core.Contracts.Services;
using GroceryStore.MainApp.Core.Models;

namespace GroceryStore.MainApp.ViewModels;

public partial class CategoryViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public CategoryViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
