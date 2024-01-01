using DevExpress.ClipboardSource.SpreadsheetML;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace GroceryStore.MainApp.Services.REST;

public class RestService
{
    public enum HeaderType
    {
        JSON,
        Image,
    }

    public static Dictionary<HeaderType, string> Headers = new()
    {
        {HeaderType.JSON, "application/json"},
        {HeaderType.Image, "image/png"},
    };

    public static async Task<string?> GetString(string hostUrl, string? urlParams, HeaderType headerType)
    {
        using HttpClient client = new();
        client.BaseAddress = new Uri(hostUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Headers[headerType]));
        try
        {
            var response = await client.GetAsync(urlParams);
            response.EnsureSuccessStatusCode();
            var resString = await response.Content.ReadAsStringAsync();
            return resString;
        }
        catch (HttpRequestException hex)
        {
            Debug.WriteLine(hex.Message);
            return null;
        }
    }

    public static async Task<Stream?> GetStream(string hostUrl, string? urlParams, HeaderType headerType)
    {
        using HttpClient client = new();
        client.BaseAddress = new Uri(hostUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Headers[headerType]));
        try
        {
            var response = await client.GetAsync(urlParams);
            response.EnsureSuccessStatusCode();
            var resStream = await response.Content.ReadAsStreamAsync();
            return resStream;
        }
        catch (HttpRequestException hex)
        {
            Debug.WriteLine(hex.Message);
            return null;
        }
    }
}
