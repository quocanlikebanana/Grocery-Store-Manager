
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;
using GroceryStore.MainApp.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace GroceryStore.MainApp.Services;

public class StatePreserveService : IStatePreserveService
{
    private const string SettingsKey = "AppStateViewModel";

    private readonly ILocalSettingsService _localSettingsService;
    private readonly INavigationService _navigationService;

    public StatePreserveService(ILocalSettingsService localSettingsService, INavigationService navigationService)
    {
        _localSettingsService = localSettingsService;
        _navigationService = navigationService;
    }

    public async Task SaveStateAsync()
    {
        var frame = _navigationService.Frame;
        if (frame?.GetPageViewModel() is ObservableRecipient pageVM)
        {
            var page = pageVM.GetType().FullName!;
            await _localSettingsService.SaveSettingAsync(SettingsKey, page);
        }
    }

    public async Task<bool> LoadStateAsync()
    {
        try
        {
            var page = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);
            if (string.IsNullOrEmpty(page))
            {
                return false;
            }
            _navigationService.NavigateTo(page);
            var frame = _navigationService.Frame;
            return true;
        }
        catch (Exception)
        {
            ApplicationData.Current.LocalSettings.Values[SettingsKey] = "";
        }
        return false;
    }
}
