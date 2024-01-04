using GroceryStore.MainApp.Strategies;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GroceryStore.MainApp.Config;


public static class Configurator
{
    public static void Config(string key, string value, ISettingStorage? settingStorage = null)
    {
        if (settingStorage == null)
        {
            settingStorage = new WindowStorage();
        }
        settingStorage.Save(key, value);
    }

    public static string? Load(string key, ISettingStorage? settingStorage = null)
    {
        if (settingStorage == null)
        {
            settingStorage = new WindowStorage();
        }
        return settingStorage.Load(key);
    }

    public static void ConfigSecure(string key, string value, ISettingStorage? settingStorage = null)
    {
        if (settingStorage == null)
        {
            settingStorage = new WindowStorage();
        }
        var entropy = new byte[20];
        var secureInBytes = Encoding.UTF8.GetBytes(value);
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(entropy);
        }
        var cypherText = ProtectedData.Protect(secureInBytes, entropy, DataProtectionScope.CurrentUser);
        var secureIn64 = Convert.ToBase64String(cypherText);
        var entropyIn64 = Convert.ToBase64String(entropy);
        settingStorage.Save(key, secureIn64);
        settingStorage.Save("Entropy" + key, entropyIn64);
    }

    public static string? LoadSecure(string key, ISettingStorage? settingStorage = null)
    {
        if (settingStorage == null)
        {
            settingStorage = new WindowStorage();
        }
        string? secure = null;
        var cyperTextIn64 = settingStorage.Load(key);
        if (cyperTextIn64 != null && cyperTextIn64.Length > 0)
        {
            var entropyIn64 = settingStorage.Load("Entropy" + key);
            if (entropyIn64 != null)
            {
                var entropyInBytes = Convert.FromBase64String(entropyIn64);
                var cyperTextInBytes = Convert.FromBase64String(cyperTextIn64);
                var secureInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                secure = Encoding.UTF8.GetString(secureInBytes);
            }
        }
        return secure;
    }

    //=======================================

    //public static void ConfigDatabase(string server, string database)
    //{
    //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    //    config.AppSettings.Settings["Server"].Value = server;
    //    config.AppSettings.Settings["Database"].Value = database;
    //    config.Save(ConfigurationSaveMode.Minimal);
    //    ConfigurationManager.RefreshSection("appSettings");
    //}

    //public static void ConfigUser(string username, string password, ISettingStorage? settingStorage = null)
    //{
    //    if (settingStorage == null)
    //    {
    //        settingStorage = new WindowStorage();
    //    }
    //    var entropy = new byte[20];
    //    var passwordInBytes = Encoding.UTF8.GetBytes(password);
    //    using (var rng = RandomNumberGenerator.Create())
    //    {
    //        rng.GetBytes(entropy);
    //    }
    //    var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
    //    var passwordIn64 = Convert.ToBase64String(cypherText);
    //    var entropyIn64 = Convert.ToBase64String(entropy);
    //    settingStorage.Save("Username", username);
    //    settingStorage.Save("Password", passwordIn64);
    //    settingStorage.Save("Entropy", entropyIn64);
    //}

    //public static string Server { get => ConfigurationManager.AppSettings["Server"] ?? ""; }
    //public static string Database { get => ConfigurationManager.AppSettings["Database"] ?? ""; }
    //public static string Username { get => ConfigurationManager.AppSettings["Username"] ?? ""; }
    //public static string Password { get => RetrivePassword(); }

    //private static string RetrivePassword()
    //{
    //    var password = "";
    //    var cyperTextIn64 = ConfigurationManager.AppSettings["Password"];
    //    if (cyperTextIn64 != null && cyperTextIn64.Length > 0)
    //    {
    //        var entropyIn64 = ConfigurationManager.AppSettings["Entropy"];
    //        if (entropyIn64 != null)
    //        {
    //            var entropyInBytes = Convert.FromBase64String(entropyIn64);
    //            var cyperTextInBytes = Convert.FromBase64String(cyperTextIn64);
    //            var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
    //            password = Encoding.UTF8.GetString(passwordInBytes);
    //        }
    //    }
    //    return password;
    //}
}
