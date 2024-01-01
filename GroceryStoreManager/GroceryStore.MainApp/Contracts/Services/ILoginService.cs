namespace GroceryStore.MainApp.Contracts.Services;

internal interface ILoginService
{
    Task Authenticate();
    string ConnectionString();
}
