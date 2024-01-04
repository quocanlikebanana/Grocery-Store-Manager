using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.MainApp.Contracts.Services;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Windows.Input;
using GroceryStore.MainApp.Config;
using GroceryStore.MainApp.Command;

namespace GroceryStore.MainApp.ViewModels.PopupVM;

public partial class LoginPopupVM : PopupVMBase
{
    private string _connectionString = string.Empty;
    private string _connectionStringSys = string.Empty;

    public LoginPopupVM(IPopupService dialogService, object? content = null) : base(dialogService, content)
    {
        LoginCommand = new DelegateCommand(Login);
        if (bool.TryParse(Configurator.Load("RememberMe"), out bool rememberme))
        {
            RememberMe = rememberme;
        }
        else
        {
            RememberMe = false;
        }
        Username = RememberMe ? Configurator.Load("Username") ?? "" : "";
        LoadPassword = () =>
        {
            return RememberMe ? Configurator.LoadSecure("Password") ?? "" : "";
        };
    }

    [ObservableProperty]
    private string _username = string.Empty;

    // password is unbind-able
    public Func<string> RetrivePassword = () => string.Empty;
    public Func<string> LoadPassword;

    [ObservableProperty]
    private bool _rememberMe = false;

    [ObservableProperty]
    private string _server = Configurator.Load("Server") ?? "";

    [ObservableProperty]
    private string _database = Configurator.Load("Database") ?? "";

    // ========================

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;
    //public bool ShouldDisplay;

    //=========================
    //=========================

    public ICommand LoginCommand { get; }

    private static async Task<bool> CheckConnection(string connectionString)
    {
        return await Task.Run(() =>
        {
            var testConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            try
            {
                testConnection.Open();
            }
            catch (Exception)
            {
                return false;
            }
            testConnection!.Close();
            return true;
        });
    }

    private async void Login(object? param)
    {
        IsLoading = true;
        var password = RetrivePassword!.Invoke();
        var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder();
        builder.DataSource = Server;
        builder.InitialCatalog = Database;
        builder.UserID = Username;
        builder.Password = password;
        builder.TrustServerCertificate = true;
        var connectionString = builder.ConnectionString;

        var checkConnection = await CheckConnection(connectionString);
        if (checkConnection == true)
        {
            // Sucess
            // Remember me
            if (RememberMe == true)
            {
                Configurator.Config("Username", Username);
                Configurator.ConfigSecure("Password", password);
            }
            else
            {
                // Remove old record
                Configurator.Config("Username", "");
                Configurator.ConfigSecure("Password", "");
            }
            Configurator.Config("Server", Server);
            Configurator.Config("Database", Database);
            Configurator.Config("RememberMe", _rememberMe.ToString());

            // For System.Data.Sqlclient
            var builderSys = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builderSys.DataSource = Server;
            builderSys.InitialCatalog = Database;
            builderSys.UserID = Username;
            builderSys.Password = password;
            builderSys.TrustServerCertificate = true;
            _connectionStringSys = builderSys.ConnectionString;
            _connectionString = connectionString;
            Accept(null);
        }
        else
        {
            // Fail
            ErrorMessage = "Failed while connecting to the database. Connection string: " + connectionString;
        }
        IsLoading = false;
    }

    //=========================
    //=========================

    public override object? GetFormData()
    {
        return Tuple.Create(_connectionString, _connectionStringSys);
    }
}
