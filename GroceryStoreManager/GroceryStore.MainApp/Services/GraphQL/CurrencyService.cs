using GroceryStore.MainApp.Config;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GroceryStore.MainApp.Services.GraphQL;

public class Latest
{
    public string? Date { get; set; }
    public string? BaseCurrency { get; set; }
    public string? QuoteCurrency { get; set; }
    public string? Quote { get; set; }
}

public class CurrencyData
{
    public Latest[]? Latest { get; set; }
    public Dictionary<string, string> getExchangeRate()
    {
        var rates = new Dictionary<string, string>();
        decimal? vndCoef = null;
        vndCoef = decimal.Parse(Latest!.First(l => l.QuoteCurrency == "VND").Quote!);
        if (vndCoef == null)
        {
            return rates;
        }
        foreach (var rate in Latest!)
        {
            if (rate.QuoteCurrency == "VND") continue;
            var exRate = (decimal)vndCoef / decimal.Parse(rate.Quote!);
            var numberFormat = new NumberFormatInfo()
            {
                CurrencySymbol = "đ",
                CurrencyDecimalDigits = 0,
                CurrencyGroupSeparator = ",",
                CurrencyPositivePattern = 3,
            };
            var vnd = exRate.ToString("c", numberFormat);
            rates.Add(rate.QuoteCurrency!, vnd);
        }
        return rates;
    }
}

public class CurrencyService
{
    private readonly string apiKey = AppConfigurate.Load("CurrencyAPI") ?? "";
    private readonly string hostUrl = "https://swop.cx/graphql";
    // because the server only allow limited type of query so no need to generalize it
    public async Task<CurrencyData?> GetCurrencyData()
    {
        var query = """
            {
            latest(baseCurrency: "EUR", quoteCurrencies: ["VND", "EUR", "USD", "JPY", "KRW"]) {
                date
                baseCurrency
                quoteCurrency
                quote
              }
            }
            """;
        var result = await GraphQLService.GetData<CurrencyData>(hostUrl, query, new KeyValuePair<string, string>("ApiKey", apiKey));
        return result;
    }
}

