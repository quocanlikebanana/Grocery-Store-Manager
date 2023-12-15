using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Temporary_Appendix;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = new SqlConnectionStringBuilder();
        builder.DataSource = "localhost\\SQLEXPRESS";
        //builder.InitialCatalog = "newestDataBase";
        builder.IntegratedSecurity = true;
        builder.TrustServerCertificate = true;
        var connectionString = builder.ConnectionString;

        GenDatabase.run(connectionString, AppDomain.CurrentDomain.BaseDirectory + "/script/maindb.sql");
        Console.WriteLine("DB Update Sucess!");
        return;
    }
}
