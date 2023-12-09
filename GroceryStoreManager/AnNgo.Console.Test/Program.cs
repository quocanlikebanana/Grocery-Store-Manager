using GroceryStore.Data.EntityFramework.Services;
using Microsoft.Data.SqlClient;

namespace AnNgo.ConsoleTest;

internal class Program
{
    public string ConnectionString
    {
        get
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLEXPRESS";
            builder.InitialCatalog = "testDataWindow";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }
    }

    static async Task Main(string[] args)
    {
        await Test1();
        await Test2();
        TestClass test = new();
        test.Test();
    }

    private static async Task Test2()
    {
        var builder = new SqlConnectionStringBuilder();
        builder.DataSource = "localhost\\SQLEXPRESS";
        builder.InitialCatalog = "testDataWindow";
        builder.IntegratedSecurity = true;
        builder.TrustServerCertificate = true;
        string connectionString = builder.ConnectionString;

        var orderDb = new OrderDetailDataService(connectionString);
        var list = await orderDb.GetAll();
    }

    static async Task Test1()
    {
        try
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLEXPRESS";
            builder.InitialCatalog = "testDataWindow";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            string connectionString = builder.ConnectionString;

            var orderDb = new OrderDataService(connectionString);
            var list = await orderDb.GetAll();
            var listCus = list.Select(x => x.Customer).ToList();
            listCus.ForEach(cus => Console.WriteLine(cus?.Name));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    }
}
