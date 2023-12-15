using System;

namespace Test_ConsoleApp
{
    internal class Program
    {
        public static async Task testOrderDetail(string connectionString)
        {

            //IDataService<Customer> cusbd = new CustomerDataService(connectionString);
            //await cusbd.Update(1,new Customer() { 
            //    Name = "vo chi trung",
            //    MoneyForPromotion= 999,
             
            //});


            IDataService<Product> db = new ProductDataService(connectionString);
            //var products = await db.FilterPrice(100,17000,1,1);
            //foreach (var item in products)
            //{
            //    Console.WriteLine($"TenSp: {item.Name} LoaiSp: {item.Type.Name}  Gia: {item.Price} SoLuong: {item.Quantity}" );
            //}
            //await db.Create(new Product()
            //{
            //    Name = "Test",
            //    Type = new ProductType()
            //    {
            //        Name = "Test",
            //    },
            //    Price = 100,
            //    Quantity = 100,
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
            await orderdbs.Delete(6);
            //Order? order = await orderdbs.Get(1);
            //Console.WriteLine(order?.Id);
            //Console.WriteLine(order?.Customer?.Name);
            //Console.WriteLine(order?.TotalPrice);
            //Console.WriteLine(order?.TotalDiscount);
            //if(order?.details != null)
            //{
            //foreach (var item in order.details)
            //{
            //    Console.WriteLine(item?.Product?.Name);
            //    Console.WriteLine(item?.Product?.Quantity);
            //    Console.WriteLine(item?.Product?.Type?.Name);
            //}

            //}

            //Console.WriteLine(order);
            //Order? order = await orderdbs.Get(1);
            //foreach (var item in order.details)
            //{
            //    Console.WriteLine(item.Product.Name);
            //    Console.WriteLine(item.Quantity);
            //}
            //await orderdbs.Create(new Order()
            //{

            //    Customer = new Customer()
            //    {
            //        Name = "An",
            //        MoneyForPromotion = 0,
            //        CouponCount = 12,
            //    },

            //    OrderDate = DateTime.Now,
            //    details = new List<OrderDetail>(){
            //        new OrderDetail() {
            //            Product = new Product() {
            //                Name = "cac dai 8m",
            //                Type = new ProductType()
            //                {
            //                    Name = "do truy",

            //                },
            //                Price = 100,
            //                Quantity = 100,
            //                BasePrice = 100,
            //            },
            //            Quantity = 20,
            //        },
            //        new OrderDetail() {
            //            ProductId = 3,
            //            Quantity = 20,
            //        }
            //    },
            //    TotalPrice = 0,
            //    TotalDiscount = 0,
            //});

            //await orderdbs.create(new order()
            //{
            //    customerid = 1,
            //    orderdate = datetime.now,
            //    totalprice = 0,
            //}) ;

        }

        static async Task Main(string[] args)
        {
            try {
                var builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost\\SQLEXPRESS";
                builder.InitialCatalog = "newestDataBase";
                builder.IntegratedSecurity = true;
                builder.TrustServerCertificate = true;
                string connectionString = builder.ConnectionString;

                await testOrderDetail(connectionString);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"{ex.InnerException?.Message}");
            }


        }
    }
}