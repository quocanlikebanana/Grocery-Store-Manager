using GroceryStore.Data.EntityFramework.Services;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Threading.Tasks;

namespace Test_ConsoleApp
{
    internal class Program
    {
        public static async Task testOrder(string connectionString) { 
            IDataService<Order> orderdbs = new OrderDataService(connectionString);
            Order? order = await orderdbs.Get(1);
            Console.WriteLine(order?.Customer?.Name);
        }

        public static async Task testCustomer(string connectionString)
        {
            IDataService<Customer> cusDBC = new CustomerDataService(connectionString);
            Customer? customer = await cusDBC.Get(1);
            if(customer != null)
            {
                Console.WriteLine(customer.Id);
                Console.WriteLine(customer.Name);
                Console.WriteLine(customer.MoneyForPromotion);
                foreach (var coupon in customer.Coupons)
                {
                    Console.WriteLine(coupon.perCoupon);
                }
            }
        }
        static async Task Main(string[] args)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQLEXPRESS";
            builder.InitialCatalog = "testDataWindow";
            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            string connectionString = builder.ConnectionString;

            IDataService<Customer> cusDBC = new CustomerDataService(connectionString);
            Customer customer = new Customer()
            {
                Name = "Test",
                Coupons = new List<Coupon> {
                        new Coupon(){ perCoupon = 69, ThresHold = 21}, 
                },
                MoneyForPromotion = 1
            };

            await cusDBC.Create(customer);


        }
    }
}
