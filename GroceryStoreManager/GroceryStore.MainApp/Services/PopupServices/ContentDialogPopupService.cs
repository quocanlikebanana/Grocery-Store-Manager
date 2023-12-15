using DevExpress.WinUI.Core.Internal;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.CustomControls;
using GroceryStore.MainApp.ViewModels.SubWindowVM;
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

    public event Action? OnWindowClose;

    public void CloseWindow()
    {
        _contentDialog?.Close();
    }

    public async void ShowWindow(object? content = null)
    {
        var viewModel = _createVM.Invoke(this, content);
        if (Activator.CreateInstance(_tView, viewModel) is not Page view)
        {
            return;
        }
        _contentDialog = new ContentDialogBase()
        {
            Content = view,
            XamlRoot = App.MainWindow.Content.XamlRoot,
        };
        await _contentDialog.ShowAsync();
    }
}
