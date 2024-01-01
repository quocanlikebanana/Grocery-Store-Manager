using DevExpress.WinUI.Core.Internal;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.CustomControls;
using GroceryStore.MainApp.ViewModels.PopupVM;
using GroceryStore.MainApp.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp.Services.PopupServices;

public class ContentDialogPopupService : IPopupService
{
    private readonly Type _tView;
    private readonly Func<IPopupService, object?, PopupVMBase> _createVM;
    private ContentDialogBase? _contentDialog;

    public ContentDialogPopupService(Type tView, Func<IPopupService, object?, PopupVMBase> createVM)
    {
        _tView = tView;
        _createVM = createVM;
    }

    public event Action<object?>? OnPopupAcceptSucess;

    public void CloseWindow()
    {
        _contentDialog?.Close();
    }

    public async Task<object?> ShowWindow(object? content = null)
    {
        object? result = null;
        var viewModel = _createVM.Invoke(this, content);
        if (Activator.CreateInstance(_tView, viewModel) is not Page view)
        {
            return result;
        }
        _contentDialog = new ContentDialogBase()
        {
            Content = view,
            XamlRoot = App.MainWindow.Content.XamlRoot,
        };
        _contentDialog.Closed += (s, e) =>
        {
            if (viewModel.Result == PopupResult.Suceed)
            {
                result = viewModel.GetFormData();
                //OnPopupAcceptSucess?.Invoke(result);
            }
        };
        await _contentDialog.ShowAsync();
        return result;
    }
}
