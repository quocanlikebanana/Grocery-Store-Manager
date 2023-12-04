using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using TemplateTest.Contracts.ViewModels;
using TemplateTest.Core.Contracts.Services;
using TemplateTest.Core.Models;

namespace TemplateTest.ViewModels;

public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public OrderViewModel(ISampleDataService sampleDataService)
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
