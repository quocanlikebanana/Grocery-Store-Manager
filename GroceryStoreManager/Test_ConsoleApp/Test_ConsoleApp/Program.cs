using System;

namespace Test_ConsoleApp
{
    internal class Program
    {
        public static async Task testOrderDetail(string connectionString)
        {

            IDataService<Product> db = new ProductDataService(connectionString);
            //await db.Create(new Product()
            //{
            //    Name = "Test",
            //    Type = new ProductType() { 
            //        Name = "Test",  
            //        Image = "test"
            //    },
            //    Price = 100,
            //    Quantity = 100,
            //    Image = "cc"
            //});
            //Product? pro3 = await db.Get(3);
            //Product? pro4 = await db.Get(4);
            //Product? pro5 = await db.Get(5);
            //Console.WriteLine(pro.Type.Name);

            //IDataService<OrderDetail> orderdbs = new OrderDetailDataService(connectionString);
            //IEnumerable<OrderDetail>? orders = await orderdbs.GetAll(1);
            //foreach (var item in orders)
            //{
            //    Console.WriteLine(item.Product.Type.Name);
            //}

            //await orderdbs.create(new orderdetail()
            //{
            //    orderid = 1,
            //    productid = 5,
            //    quantity = 1,
            //}) ;

            IDataService<Order> orderdbs = new OrderDataService(connectionString);
            //Order? order = await orderdbs.Get(1);
            //foreach (var item in order.details)
            //{   
            //    Console.WriteLine(item.Product.Name);
            //    Console.WriteLine(item.Quantity);
            //}
            await orderdbs.Create(new Order()
            {
                CustomerID = 3,
                OrderDate = DateTime.Now,
                details = new List<OrderDetail>(){
                    new OrderDetail() {
                        ProductId = 2,
                        OrderId = null,
                        Quantity = 20,
                    }
                }
            });

            //await orderdbs.create(new order()
            //{
            //    customerid = 1,
            //    orderdate = datetime.now,
            //    totalprice = 0,
            //}) ;

        }

        public static async Task testOrder(string connectionString) { 
            IDataService<Order> orderdbs = new OrderDataService(connectionString);
            IEnumerable<Order>? orders = await orderdbs.GetAll();
            foreach (var order in orders)
            {
                Console.WriteLine(order.Customer.Name);
            }
        }

        public static async Task testCustomer(string connectionString)
        {
            IDataService<Customer> cusDBC = new CustomerDataService(connectionString);
            IEnumerable<Customer>? customers = await cusDBC.GetAll() ;

            foreach (var cus in customers)
            {
                Console.WriteLine(cus.Name);
            }
    
        }
        static async Task Main(string[] args)
        {
            try {
                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost\\SQLEXPRESS";
                builder.InitialCatalog = "testDataWindow";
                builder.IntegratedSecurity = true;
                builder.TrustServerCertificate = true;
                string connectionString = builder.ConnectionString;

                await testOrderDetail(connectionString);
            } catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }


        }
    }
}