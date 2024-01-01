using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.ViewModels;

using Microsoft.UI.Xaml;

namespace GroceryStore.MainApp.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;
    private readonly IStatePreserveService _statePreserveService;

    public DefaultActivationHandler(INavigationService navigationService, IStatePreserveService statePreserveService)
    {
        _navigationService = navigationService;
        _statePreserveService = statePreserveService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    // where first navigate begins (even before load)
    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo("", args.Arguments);

        await Task.CompletedTask;
    }
}
