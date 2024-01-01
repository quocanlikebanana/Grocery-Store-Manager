using Microsoft.IdentityModel.Protocols;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace GroceryStore.MainApp.Config;

public static class AppConfigurate
{
    public static void Config(string key, string value)
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings[key].Value = value;
        config.Save(ConfigurationSaveMode.Minimal);
        ConfigurationManager.RefreshSection("appSettings");
    }

    public static string? Load(string key)
    {
        return ConfigurationManager.AppSettings[key];
    }

    public static void ConfigSecure(string key, string value)
    {
        var entropy = new byte[20];
        var secureInBytes = Encoding.UTF8.GetBytes(key);
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(entropy);
        }
        var cypherText = ProtectedData.Protect(secureInBytes, entropy, DataProtectionScope.CurrentUser);
        var secureIn64 = Convert.ToBase64String(cypherText);
        var entropyIn64 = Convert.ToBase64String(entropy);
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings[key].Value = secureIn64;
        config.AppSettings.Settings["Entropy" + key].Value = entropyIn64;
        config.Save(ConfigurationSaveMode.Minimal);
        ConfigurationManager.RefreshSection("appSettings");
    }

    public static string? LoadSecure(string key)
    {
        string? secure = null;
        var cyperTextIn64 = ConfigurationManager.AppSettings[key];
        if (cyperTextIn64 != null && cyperTextIn64.Length > 0)
        {
            var entropyIn64 = ConfigurationManager.AppSettings["Entropy" + key];
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

    public static void ConfigDatabase(string server, string database)
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["Server"].Value = server;
        config.AppSettings.Settings["Database"].Value = database;
        config.Save(ConfigurationSaveMode.Minimal);
        ConfigurationManager.RefreshSection("appSettings");
    }

    public static void ConfigUser(string username, string password)
    {
        var entropy = new byte[20];
        var passwordInBytes = Encoding.UTF8.GetBytes(password);
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(entropy);
        }
        var cypherText = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
        var passwordIn64 = Convert.ToBase64String(cypherText);
        var entropyIn64 = Convert.ToBase64String(entropy);
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["Username"].Value = username;
        config.AppSettings.Settings["Password"].Value = passwordIn64;
        config.AppSettings.Settings["Entropy"].Value = entropyIn64;
        config.Save(ConfigurationSaveMode.Minimal);
        ConfigurationManager.RefreshSection("appSettings");
    }

    public static string Server { get => ConfigurationManager.AppSettings["Server"] ?? ""; }
    public static string Database { get => ConfigurationManager.AppSettings["Database"] ?? ""; }
    public static string Username { get => ConfigurationManager.AppSettings["Username"] ?? ""; }
    public static string Password { get => RetrivePassword(); }

    private static string RetrivePassword()
    {
        var password = "";
        var cyperTextIn64 = ConfigurationManager.AppSettings["Password"];
        if (cyperTextIn64 != null && cyperTextIn64.Length > 0)
        {
            var entropyIn64 = ConfigurationManager.AppSettings["Entropy"];
            if (entropyIn64 != null)
            {
                var entropyInBytes = Convert.FromBase64String(entropyIn64);
                var cyperTextInBytes = Convert.FromBase64String(cyperTextIn64);
                var passwordInBytes = ProtectedData.Unprotect(cyperTextInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                password = Encoding.UTF8.GetString(passwordInBytes);
            }
        }
        return password;
    }
}
