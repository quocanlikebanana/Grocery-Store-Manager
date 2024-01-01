using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace GroceryStore.MainApp.Contracts.Services;

public interface IStatePreserveService
{
    Task SaveStateAsync();

    Task<bool> LoadStateAsync();
}
