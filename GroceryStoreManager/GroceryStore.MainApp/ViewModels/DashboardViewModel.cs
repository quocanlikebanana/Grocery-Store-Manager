using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Strategies.GraphQL;
using GroceryStore.MainApp.Strategies.REST;
using System.Collections.ObjectModel;

namespace GroceryStore.MainApp.ViewModels;

public partial class DashboardViewModel : ObservableRecipient, INavigationAware
{
    private readonly WeatherService _weatherService;
    private readonly CurrencyService _currencyService;
    private readonly IStatisticService _statisticService;
    private readonly IDataService<Product> _productDataService;

    public DashboardViewModel()
    {
        _weatherService = new WeatherService();
        _currencyService = new CurrencyService();
        _statisticService = App.GetService<IStatisticService>();
        _productDataService = App.GetService<IDataService<Product>>();
    }

    [ObservableProperty]
    private ObservableCollection<WeatherData?> _weatherDatas = new();

    [ObservableProperty]
    private int _totalProduct;

    [ObservableProperty]
    private Dictionary<string, string> _exchangeRates = new();

    [ObservableProperty]
    private int _newOrderInLast7Days;

    [ObservableProperty]
    private int _newOrderInMonth;

    [ObservableProperty]
    private List<Product> _top5OutOfStock = new();

    private async Task Load()
    {
        var t1 = Task.Run(async () =>
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
         });
        var t2 = _currencyService.GetCurrencyData();
        var t3 = _productDataService.GetAll();
        var currentDate = DateTime.Now;
        var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        var t4 = _statisticService.GetNumberOf(currentDate.AddDays(-7), currentDate);
        var t5 = _statisticService.GetNumberOf(startOfMonth, currentDate);
        // Await section:
        Task.WaitAll(t1);
        ExchangeRates = (await t2)!.getExchangeRate() ?? new();
        var products = await t3;
        TotalProduct = products.Count();
        Top5OutOfStock = (from p in products orderby p.Quantity select p).Take(5).ToList();
        NewOrderInLast7Days = await t4;
        NewOrderInMonth = await t5;
    }

    public async void OnNavigatedTo(object parameter)
    {
        await Load();

    }

    public void OnNavigatedFrom()
    {
    }
}
