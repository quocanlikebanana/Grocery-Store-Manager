using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Core.Services;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.Helpers;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Windows.Storage.Pickers;

namespace GroceryStore.MainApp.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }

    private readonly IExcelReaderService _excelReaderService;
    private readonly IBackupService _backupService;
    private const string EM_DLL = "GroceryStoreManager.ExcelManager.dll";
    private const string BU_DLL = "GroceryStore.Data.Backup.dll";

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        ImportExcelCommand = new AsyncRelayCommand(ImportExcel);
        SaveBackupCommand = new AsyncRelayCommand(SaveBackup);
        LoadBackupCommand = new AsyncRelayCommand(LoadBackup);
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public ICommand ImportExcelCommand { get; }

    public ICommand SaveBackupCommand { get; }
    public ICommand LoadBackupCommand { get; }

    private async Task ImportExcel()
    {
        string[] exts = { ".xls", ".xlsx", ".xlsm" };
        await FileOpenMethod(async (filePath) =>
        {
            try
            {
                // this bitch use Sytem.Data
                var connectionString = App.GetService<ILoginService>().ConnectionStringSys();
                var _EMType = DynamicPluginSupport.DynamicPlugin.GetImplement<IExcelReaderService>(EM_DLL);
                var exrService = (IExcelReaderService)Activator.CreateInstance(_EMType!, filePath, connectionString)!;
                await exrService.run();
            }
            catch (Exception ex)
            {
                var popupError = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Error);
                await popupError.ShowWindow(ex.Message);
            }
        }, null, exts);
    }

    private async Task SaveBackup()
    {
        await FolderOpenMethod(async (filePath) =>
        {
            try
            {
                var connectionString = App.GetService<ILoginService>().ConnectionString();
                var _EMType = DynamicPluginSupport.DynamicPlugin.GetImplement<IBackupService>(BU_DLL);
                var buService = (IBackupService)Activator.CreateInstance(_EMType!, connectionString)!;
                buService.BackupDatabase(filePath);
            }
            catch (Exception ex)
            {
                var popupError = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Error);
                await popupError.ShowWindow(ex.Message);
            }
        }, null);
    }

    private async Task LoadBackup()
    {
        string[] exts = { ".bak" };
        await FileOpenMethod(async (filePath) =>
        {
            try
            {
                var connectionString = App.GetService<ILoginService>().ConnectionString();
                var _EMType = DynamicPluginSupport.DynamicPlugin.GetImplement<IBackupService>(BU_DLL);
                var buService = (IBackupService)Activator.CreateInstance(_EMType!, connectionString)!;
                buService.RestoreDatabase(filePath);
            }
            catch (Exception ex)
            {
                var popupError = PopupServiceFactoryMethod.Get(PopupType.ContentDialog, PopupContent.Error);
                await popupError.ShowWindow(ex.Message);
            }
        }, null, exts);
    }

    private static async Task FileOpenMethod(Action<string> choosen, Action? cancel = null, string[]? exts = null)
    {
        var openPicker = new FileOpenPicker();

        // Freezes the screen
        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = WindowHelper.GetWindowForElement(this);
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your file picker
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        if (exts == null)
        {
            openPicker.FileTypeFilter.Add("*");
        }
        else
        {
            foreach (var e in exts)
            {
                openPicker.FileTypeFilter.Add(e);
            }
        }

        // Open the picker for the user to pick a file
        var file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            //includes name and directory
            var filePath = file.Path;
            choosen.Invoke(filePath);
        }
        else
        {
            // not select file
            cancel?.Invoke();
        }
    }

    private static async Task FolderOpenMethod(Action<string> choosen, Action? cancel = null)
    {
        var openPicker = new FolderPicker();

        // Freezes the screen
        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = WindowHelper.GetWindowForElement(this);
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your file picker
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a file
        var folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            //includes name and directory
            var filePath = folder.Path;
            choosen.Invoke(filePath);
        }
        else
        {
            // not select file
            cancel?.Invoke();
        }
    }

    private static async Task FileSaveMethod(Action<string> choosen, Action? cancel = null, string[]? exts = null)
    {
        var openPicker = new FileSavePicker();

        // Freezes the screen
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.SuggestedFileName = "backup";
        if (exts == null)
        {
            openPicker.FileTypeChoices.Add("Any", new List<string>() { "*" });
        }
        else
        {
            var listExt = new List<string>();
            foreach (var e in exts)
            {
                listExt.Add(e);
            }
            openPicker.FileTypeChoices.Add("Optional", listExt);
        }
        var file = await openPicker.PickSaveFileAsync();
        if (file != null)
        {
            var filePath = file.Path;
            choosen.Invoke(filePath);
        }
        else
        {
            cancel?.Invoke();
        }
    }
}
