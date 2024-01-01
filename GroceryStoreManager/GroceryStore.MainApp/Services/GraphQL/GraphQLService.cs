using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GroceryStore.MainApp.Services.GraphQL;

public class GraphQLService
{
    public static async Task<T?> GetData<T>(string hostUrl, string query, KeyValuePair<string, string> authheader)
    {
        var graphQLHttpClientOptions = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(hostUrl)
        };
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authheader.Key, authheader.Value);
        var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);
        var currencyRequest = new GraphQLRequest
        {
            Query = query
        };
        var graphQLResponse = await graphQLClient.SendQueryAsync<T>(currencyRequest);
        return graphQLResponse.Data;
    }
}

