using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using TemplateTest.Contracts.ViewModels;
using TemplateTest.Core.Contracts.Services;
using TemplateTest.Core.Models;

namespace TemplateTest.ViewModels;

// ObservableRecipient = ObservableObject + IMessager
public partial class CustomerViewModel : /*ObservableRecipient,*/ ObservableObject, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private SampleOrder? selected;

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    public CustomerViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        SampleItems.Clear();

        // TODO2: Replace with real data.
        var data = await _sampleDataService.GetListDetailsDataAsync();

        foreach (var item in data)
        {
            SampleItems.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public void EnsureItemSelected()
    {
        Selected ??= SampleItems.First();
    }
}
