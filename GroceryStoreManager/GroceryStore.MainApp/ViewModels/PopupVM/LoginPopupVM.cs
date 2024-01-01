using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.MainApp.Contracts.Services;
using Microsoft.Data.SqlClient;
using System.Windows.Input;
using GroceryStore.MainApp.Config;
using GroceryStore.MainApp.Command;

namespace GroceryStore.MainApp.ViewModels.PopupVM;

public partial class LoginPopupVM : PopupVMBase
{
    private string _connectionString = string.Empty;

    public LoginPopupVM(IPopupService dialogService, object? content = null) : base(dialogService, content)
    {
        LoginCommand = new DelegateCommand(Login);
        if (bool.TryParse(AppConfigurate.Load("RememberMe"), out bool rememberme))
        {
            RememberMe = rememberme;
        }
        else
        {
            RememberMe = false;
        }
        Username = RememberMe ? AppConfigurate.Username : "";
        LoadPassword = () =>
        {
            return RememberMe ? AppConfigurate.Password : "";
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
    private string _server = AppConfigurate.Server;

    [ObservableProperty]
    private string _database = AppConfigurate.Database;

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
            // Make the connecting more realistic
            Thread.Sleep(2000);
            var testConnection = new SqlConnection(connectionString);
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
        var builder = new SqlConnectionStringBuilder();
        builder.DataSource = $"{Server}\\SQLEXPRESS";
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
                AppConfigurate.ConfigUser(Username, password);
            }
            else
            {
                // Remove old record
                AppConfigurate.ConfigUser("", "");
            }
            AppConfigurate.ConfigDatabase(Server, Database);
            AppConfigurate.Config("RememberMe", _rememberMe.ToString());
            _connectionString = connectionString;
            Accept(null);
        }
        else
        {
            // Fail
            ErrorMessage = "Failed while connecting to the database";
        }
        IsLoading = false;
    }

    //=========================
    //=========================

    public override object? GetFormData()
    {
        return _connectionString;
    }
}
