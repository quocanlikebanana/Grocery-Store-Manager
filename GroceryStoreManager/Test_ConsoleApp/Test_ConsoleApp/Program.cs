using GroceryStore.Data.EntityFramework.Services;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.Data.SqlClient;

namespace Test_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLEXPRESS";
            builder.InitialCatalog = "testDataWindow";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            string connectionString = builder.ConnectionString;
            IDataService<Customer> dataService = new CustomerDataService(connectionString);

            var customer = dataService.Get(1).Result;
            Console.WriteLine(customer.Id);
            Console.WriteLine(customer.Name);
            Console.WriteLine(customer.MoneyForPromotion);
            foreach (var coupon in customer.Coupons)
            {
                Console.WriteLine(coupon.perCoupon);
            }
        }
    }
}
