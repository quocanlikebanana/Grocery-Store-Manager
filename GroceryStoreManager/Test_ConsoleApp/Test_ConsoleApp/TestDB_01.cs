using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Data.EntityFramework.Services;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.Data.SqlClient;

namespace Test_ConsoleApp;
public class TestDB_01
{
    public static async Task testOrder(string connectionString)
    {
        IDataService<Order> orderdbs = new OrderDataService(connectionString);
        Order? order = await orderdbs.Get(1);
        Console.WriteLine(order?.Customer?.Name);
    }

    public static async Task testCustomer(string connectionString)
    {
        IDataService<Customer> cusDBC = new CustomerDataService(connectionString);
        Customer? customer = await cusDBC.Get(1);
        if (customer != null)
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
    public static async Task Test()
    {
        //GenDatabase.run("Data Source=ADMIN\\SQLEXPRESS;Integrated Security=True;TrustServerCertificate=True", AppDomain.CurrentDomain.BaseDirectory + "/script/maindb (1).sql");

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
        var res = await cusDBC.Get(3);
        var res2 = await cusDBC.GetAll();
        Console.WriteLine(res);
        Console.WriteLine(res2);
    }
}
