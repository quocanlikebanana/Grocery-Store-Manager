using GroceryStore.MainApp.Config;
using Newtonsoft.Json;

namespace GroceryStore.MainApp.Services.REST;



public class WeatherData
{
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("weather")]
    private List<WeatherInfo>? Weathers { get; set; }
    // only get the first one
    public WeatherInfo? Weather => Weathers!.FirstOrDefault();
    public string? Description => Weather?.Description;
    [JsonProperty("main")]
    public WeatherMain? Main { get; set; }
    public decimal? Temp
    {
        get
        {
            if (Main == null || Main.Temp == null) return null;
            var d = decimal.Parse(Main.Temp);
            return decimal.Round(d, 1);
        }
    }
}

public class WeatherInfo
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("main")]
    public string? Main { get; set; }
    [JsonProperty("description")]
    public string? Description { get; set; }
    [JsonProperty("icon")]
    public string? IconCode { get; set; }
}

public class WeatherMain
{
    [JsonProperty("temp")]
    public string? Temp { get; set; }
    [JsonProperty("humidity")]
    public string? Humidity { get; set; }
}



public class WeatherService
{
    private readonly string apiKey = AppConfigurate.Load("WeatherAPI") ?? "";
    private readonly string hostUrl = "https://api.openweathermap.org/data/2.5/weather";
    private string getUrlParam(string cityName) => $"?q={cityName}&appid={apiKey}&lang=vi&units=metric";
    private string getImageUrl(string code) => $"https://openweathermap.org/img/wn/{code}@2x.png";

    public async Task<WeatherData?> GetWeatherData(string cityName)
    {
        WeatherData? data = null;
        var stringData = await RestService.GetString(hostUrl, getUrlParam(cityName), RestService.HeaderType.JSON);
        if (stringData != null)
        {
            data = JsonConvert.DeserializeObject<WeatherData>(stringData);
        }
        return data;
    }
}
