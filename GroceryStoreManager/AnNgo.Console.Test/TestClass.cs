using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Data.EntityFramework.Services;
using Microsoft.Data.SqlClient;

namespace AnNgo.Console.Test;

public class TestClass
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

    public void Test()
    {
        var connectionString = ConnectionString;
        var orderDetailTest = new OrderDetailDataService(connectionString);

        
    }
}
