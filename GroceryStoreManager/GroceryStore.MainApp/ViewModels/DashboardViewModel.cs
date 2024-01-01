using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Services.GraphQL;
using GroceryStore.MainApp.Services.REST;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Security.Policy;

namespace GroceryStore.MainApp.ViewModels;




public partial class DashboardViewModel : ObservableRecipient, INavigationAware
{
    private readonly WeatherService _weatherService;
    private readonly CurrencyService _currencyService;
    private readonly IStatisticService _statisticService;

    public DashboardViewModel()
    {
        _weatherService = new WeatherService();
        _currencyService = new CurrencyService();
        _statisticService = App.GetService<IStatisticService>();
    }

    [ObservableProperty]
    private ObservableCollection<WeatherData?> _weatherDatas = new();

    [ObservableProperty]
    private int _totalProduct;

    [ObservableProperty]
    private Dictionary<string, string> _exchangeRates = new();

    [ObservableProperty]
    private int _newOrderInWeek;

    [ObservableProperty]
    private int _newOrderInMonth;

    private async Task Load()
    {
        // The real names are taken from the api call
        string[] cities = {
            "Ho Chi Minh City",
            "ha noi",
            "da nang",
            "can tho",
            "nha trang",
        };
        _weatherDatas.Clear();
        foreach (var city in cities)
        {
            _weatherDatas.Add(await _weatherService.GetWeatherData(city));
        }
        ExchangeRates = (await _currencyService.GetCurrencyData())?.getExchangeRate() ?? new();
    }

    public async void OnNavigatedTo(object parameter)
    {
        await Load();

    }

    public void OnNavigatedFrom()
    {
    }
}
