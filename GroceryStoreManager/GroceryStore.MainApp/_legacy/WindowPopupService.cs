using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Helpers;
using GroceryStore.MainApp.ViewModels.SubWindowVM;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp._legacy;


public class WindowPopupService : IPopupService
{
    private readonly Type _tView;
    private readonly Func<IPopupService, object?, PopupVMBase> _createVM;

    private Window? _subWindow = null;

    public WindowPopupService(Type TView, Func<IPopupService, object?, PopupVMBase> createVM)
    {
        _tView = TView;
        _createVM = createVM;
    }

    public event Action<object?>? OnPopupAcceptSucess;

    public void CloseWindow()
    {
        _subWindow?.Close();
    }

    public void ShowWindow(object? content = null)
    {
        if (_subWindow != null)
        {
            return;
        }
        var viewModel = _createVM.Invoke(this, content);
        if (Activator.CreateInstance(_tView, viewModel) is not Page view)
        {
            return;
        }

        // Call this method with AppWindowPresenterKind.Default as the parameter to apply an OverlappedPresenter.
        // https://learn.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.windowing.appwindow.setpresenter?view=windows-app-sdk-1.4#microsoft-ui-windowing-appwindow-setpresenter(microsoft-ui-windowing-appwindowpresenterkind)
        // Setting IsModal only works when the Window has an owner window.
        // https://github.com/microsoft/WindowsAppSDK/issues/3258
        // https://github.com/microsoft/WindowsAppSDK/discussions/3680
        _subWindow = new Window();
        var hSubWnd = WinRT.Interop.WindowNative.GetWindowHandle(_subWindow);
        var subWindowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hSubWnd);
        var appSubWindow = AppWindow.GetFromWindowId(subWindowId);
        var presenter = appSubWindow.Presenter as OverlappedPresenter;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        IntPrHelper.SetWindowLong(hSubWnd, IntPrHelper.GWL_HWNDPARENT, hWnd);
        presenter!.IsModal = true;
        presenter!.IsAlwaysOnTop = true;
        presenter!.IsResizable = false;
        // https://github.com/dotnet/maui/issues/15142
        //presenter.SetBorderAndTitleBar(false, false);

        _subWindow.Content = view;
        _subWindow.Closed += (sender, e) =>
        {
            // Prevent main window auto close when child close
            IntPrHelper.EnableWindow(hWnd, true);
            if (viewModel.Result == PopupResult.Suceed)
            {
                var data = viewModel.GetFormData();
                OnPopupAcceptSucess?.Invoke(data);
            }
        };

        _subWindow.Activate();
        appSubWindow.Show();
    }
}
