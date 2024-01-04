using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.WinUI.Charts;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.ViewModels;
using System.Windows.Input;

namespace GroceryStore.MainApp.ViewModels;

public partial class StatisticViewModel : ObservableRecipient, INavigationAware
{
    private readonly IStatisticService _statisticService;
    private readonly IDataService<Product> _productDataService;

    // Init
    public readonly List<TimeSpanUnit> TimeUnits = new()
    {
        new TimeSpanUnit( TimeSpan.FromDays(1), "Day"),
        new TimeSpanUnit( TimeSpan.FromDays(7), "Week"),
        new TimeSpanUnit( TimeSpan.FromDays(30), "Month"),
        new TimeSpanUnit( TimeSpan.FromDays(365), "Year"),
    };

    [ObservableProperty]
    private List<Product> _productList = new();

    public readonly List<int> Tops = new()
    {
        3,4,5,6,7,8,9,10
    };

    public StatisticViewModel()
    {
        _statisticService = App.GetService<IStatisticService>();
        _productDataService = App.GetService<IDataService<Product>>();
        SelectedUnitPR = TimeUnits[0];
        SelectedUnitPrQ = TimeUnits[0];
        SelectedUnitTS = TimeUnits[0];
        SelectedTop = Tops[0];
        ReLoadPR = new DelegateCommand(async obj => await LoadPR());
        ReLoadPrQ = new DelegateCommand(async obj => await LoadPrQ());
        ReLoadTS = new DelegateCommand(async obj => await LoadTS());
    }

    public async void OnNavigatedTo(object parameter)
    {
        var t1 = _productDataService.GetAll();
        // Await
        await LoadPR();
        await LoadPrQ();
        await LoadTS();
        ProductList = new List<Product>(await t1);
    }

    public void OnNavigatedFrom()
    {
    }

    // === Section PR === START

    private async Task LoadPR()
    {
        var t1 = ArgumentValuePairDate<double>.SplitRange(StartDatePR, EndDatePR, SelectedUnitPR, _statisticService.GetTotalRevenue);
        var t2 = ArgumentValuePairDate<double>.SplitRange(StartDatePR, EndDatePR, SelectedUnitPR, _statisticService.GetTotalProfit);
        var t3 = _statisticService.GetTotalRevenue();
        var t4 = _statisticService.GetTotalProfit();
        var t7 = _statisticService.GetAverageRevenue();
        var t8 = _statisticService.GetAverageProfit();
        // Await
        TotalRevenueData = await t1;
        TotalProfitData = await t2;
        TotalProfit = await t4;
        TotalRevenue = await t3;
        AvgProfit = await t7;
        AvgRevenue = await t8;
        var maxValueChart = Math.Max(TotalRevenueData.Max(d => d.Value), TotalProfitData.Max(d => d.Value));
        ChartInfoPR = new ChartInfo(maxValueChart * 0.2, SelectedUnitPR.MeasureUnit);
    }

    [ObservableProperty]
    private DateTime _startDatePR = DateTime.Now.AddDays(-7);

    [ObservableProperty]
    private DateTime _endDatePR = DateTime.Now;

    [ObservableProperty]
    private TimeSpanUnit _selectedUnitPR = new TimeSpanUnit(TimeSpan.FromDays(1), "Day");

    [ObservableProperty]
    private List<ArgumentValuePairDate<double>> _totalProfitData = new();

    [ObservableProperty]
    private List<ArgumentValuePairDate<double>> _totalRevenueData = new();

    [ObservableProperty]
    private double _totalRevenue = 0;

    [ObservableProperty]
    private double _totalProfit = 0;

    [ObservableProperty]
    private double _avgRevenue = 0;

    [ObservableProperty]
    private double _avgProfit = 0;

    [ObservableProperty]
    private ChartInfo _chartInfoPR = new();

    public ICommand ReLoadPR { get; }

    // === Section PR === END

    // === Section Product Quantity === START

    [ObservableProperty]
    private DateTime _startDatePrQ = DateTime.Now.AddDays(-7);

    [ObservableProperty]
    private DateTime _endDatePrQ = DateTime.Now;

    [ObservableProperty]
    private TimeSpanUnit _selectedUnitPrQ = new TimeSpanUnit(TimeSpan.FromDays(1), "Day");

    [ObservableProperty]
    private List<ArgumentValuePairDate<int>> _productQuantityData = new();

    [ObservableProperty]
    private double _totalProductQuantity = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(NotSelectedProduct))]
    private Product? _selectedProduct;

    [ObservableProperty]
    private ChartInfo _chartInfoPrQ = new();

    public bool NotSelectedProduct => SelectedProduct == null;

    private async Task LoadPrQ()
    {
        if (SelectedProduct != null)
        {
            var t1 = ArgumentValuePairDate<int>.SplitRange(StartDatePrQ, EndDatePrQ, SelectedUnitPrQ, async (s, e) => await _statisticService.ProductSale(SelectedProduct, s, e));
            var t2 = _statisticService.ProductSale(SelectedProduct);
            // Await
            ProductQuantityData = await t1;
            TotalProductQuantity = await t2;
            ChartInfoPrQ = new(ProductQuantityData.Max(q => q.Value), SelectedUnitPrQ.MeasureUnit);
        }
    }

    public ICommand ReLoadPrQ { get; }


    // === Section Product Quantity === END

    // === Section Top Seller === START

    [ObservableProperty]
    private TimeSpanUnit _selectedUnitTS = new TimeSpanUnit(TimeSpan.FromDays(1), "Day");

    [ObservableProperty]
    private List<ArgumentValuePair<string, int>> _topSellerData = new();

    [ObservableProperty]
    private int _selectedTop = 3;

    [ObservableProperty]
    private ChartInfo _chartInfoTS = new();

    private async Task LoadTS()
    {
        var startDateTS = DateTime.Now.Subtract(SelectedUnitTS.Value);
        var endDateTS = DateTime.Now;
        var t1 = _statisticService.SoldProducts(startDateTS, endDateTS);
        // Await
        var data = (await t1).OrderByDescending(d => d.Item2);
        var dataRes = data.Take(SelectedTop).Select(d => new ArgumentValuePair<string, int>(d.Item1.Name, d.Item2)).ToList();
        dataRes.Add(new ArgumentValuePair<string, int>("Others", data.Skip(SelectedTop).Sum(d => d.Item2)));
        TopSellerData = dataRes;
    }

    public ICommand ReLoadTS { get; }

    // === Section Top Saler === END

    // ====================================


}

public class ChartInfo
{
    public ChartInfo() { }
    public ChartInfo(double marignY, DateTimeMeasureUnit measureUnit)
    {
        MarignY = marignY;
        MeasureUnit = measureUnit;
    }

    public double MarignY { get; set; }
    public DateTimeMeasureUnit MeasureUnit { get; set; }
}


public class TimeSpanUnit
{
    public TimeSpanUnit()
    {
    }
    public TimeSpanUnit(TimeSpan value, string name)
    {
        Value = value;
        Name = name;
    }
    public TimeSpan Value { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeMeasureUnit MeasureUnit
    {
        get
        {
            if (Name == "Day") return DateTimeMeasureUnit.Day;
            if (Name == "Week") return DateTimeMeasureUnit.Week;
            if (Name == "Month") return DateTimeMeasureUnit.Month;
            if (Name == "Year") return DateTimeMeasureUnit.Year;
            return DateTimeMeasureUnit.Day;
        }
    }
}

public class ArgumentValuePair<T, U>
{
    public ArgumentValuePair(T? argument, U? value)
    {
        Argument = argument;
        Value = value;
    }
    public T? Argument { get; set; }
    public U? Value { get; set; }
}

public class ArgumentValuePairDate<T>
{
    public ArgumentValuePairDate(T? value, DateTime? argument)
    {
        Value = value;
        Argument = argument;
    }
    public T? Value { get; set; }
    public DateTime? Argument { get; set; }

    public static async Task<List<ArgumentValuePairDate<T>>> SplitRange(DateTime start, DateTime end, TimeSpanUnit tsUnit, Func<DateTime, DateTime, Task<T>> cb)
    {
        // check valid date
        if (end < start) return new List<ArgumentValuePairDate<T>>();
        // check if its too many record
        var ts = tsUnit.Value;
        if (end.Subtract(start).Divide(ts) >= 30)
        {
            ts = end.Subtract(start).Divide(30);
        }
        DateTime next;
        start = start.Date;
        var result = new List<ArgumentValuePairDate<T>>();
        while (start < end)
        {
            //https://stackoverflow.com/questions/902789/how-to-get-the-start-and-end-times-of-a-day
            next = start.Add(ts).AddTicks(-1);
            // Add to list can't be asynchronous
            //https://stackoverflow.com/questions/69827904/adding-items-asynchronously-to-a-list-within-an-async-method-in-c-sharp-net
            var chunk = await cb.Invoke(start, next);
            result.Add(new ArgumentValuePairDate<T>(chunk, start));
            start = start.Add(ts);
        }
        return result;
    }
}
