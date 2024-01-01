using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Services.PopupServices;
using Microsoft.Data.SqlClient;

namespace GroceryStore.MainApp.Services
{
    public class LoginService : ILoginService
    {
        private string _connectionString = string.Empty;
        public LoginService()
        {
        }

        public async Task Authenticate()
        {
            // If can't connect => GO TO Services and Refresh, Restart some of the SQL services
            var loginPopup = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Login);
            var result = await loginPopup.ShowWindow();
            _connectionString = (string)result!;

            //// TODO: Connection string configuration
            //var builder = new SqlConnectionStringBuilder();
            //builder.DataSource = "localhost\\SQLEXPRESS";
            ////builder.InitialCatalog = "newestDataBase";
            //builder.InitialCatalog = "testDataWindow";
            //builder.IntegratedSecurity = true;
            //builder.TrustServerCertificate = true;
            //var connectionString = builder.ConnectionString;
        }

        public string ConnectionString()
        {
            return _connectionString;
        }
    }
}
