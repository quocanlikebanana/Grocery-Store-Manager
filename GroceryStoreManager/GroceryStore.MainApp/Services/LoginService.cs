using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Strategies.PopupServices;
using Microsoft.Data.SqlClient;

namespace GroceryStore.MainApp.Strategies
{
    public class LoginService : ILoginService
    {
        private string _connectionString = string.Empty;
        private string _connectionStringSys = string.Empty;
        public LoginService()
        {
        }

        public async Task Authenticate()
        {
            // If can't connect => GO TO Services and Refresh, Restart some of the SQL services
            var loginPopup = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Login);
            var result = await loginPopup.ShowWindow() as Tuple<string, string>;
            _connectionString = result!.Item1;
            _connectionStringSys = result!.Item2;
        }

        public string ConnectionString() => _connectionString;
        public string ConnectionStringSys() => _connectionStringSys;
    }
}
