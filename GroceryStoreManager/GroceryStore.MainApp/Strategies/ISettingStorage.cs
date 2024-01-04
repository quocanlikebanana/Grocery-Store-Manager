using System.Configuration;
using Windows.Storage;

namespace GroceryStore.MainApp.Strategies;

public interface ISettingStorage
{
    void Save(string key, string value);
    string? Load(string key);
}

public class WindowStorage : ISettingStorage
{
    public void Save(string key, string value)
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        localSettings.Values[key] = value;
    }
    public string? Load(string key)
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        return localSettings.Values[key] as string;
    }
}

public class AppconfigStorage : ISettingStorage
{
    public void Save(string key, string value)
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings[key].Value = value;
        config.Save(ConfigurationSaveMode.Minimal);
        ConfigurationManager.RefreshSection("appSettings");
    }
    public string? Load(string key)
    {
        return ConfigurationManager.AppSettings[key];
    }
}


