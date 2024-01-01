using DynamicPluginSupport;
using GroceryStore.Data.EntityFramework.Services;
using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using GroceryStore.MainApp.Activation;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Core.Contracts.Services;
using GroceryStore.MainApp.Core.Services;
using GroceryStore.MainApp.Models;
using GroceryStore.MainApp.Services;
using GroceryStore.MainApp.ViewModels;
using GroceryStore.MainApp.Views;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace GroceryStore.MainApp;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        var host = (Current as App)!.Host;
        var obj = host.Services.GetService(typeof(T));
        if (obj is not T serviecRes)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }
        return serviecRes;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IStatePreserveService, StatePreserveService>();

            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IFileService, FileService>();

            //var order_DataServiceImplements = DynamicPlugin.GetImplements<IDataService<Order>>().ToList();
            //var coupon_DataServiceImplements = DynamicPlugin.GetImplements<IDataService<Coupon>>().ToList();
            //var customer_DataServiceImplements = DynamicPlugin.GetImplements<IDataService<Customer>>().ToList();
            //var orderDetail_DataServiceImplements = DynamicPlugin.GetImplements<IDataService<OrderDetail>>().ToList();
            //var product_DataServiceImplements = DynamicPlugin.GetImplements<IDataService<Product>>().ToList();
            //var productType_DataServiceImplements = DynamicPlugin.GetImplements<IDataService<ProductType>>().ToList();

            //coupon_DataServiceImplements.ForEach(ServiceType => services.AddSingleton(typeof(IDataService<Coupon>), x => ActivatorUtilities.CreateInstance(x, ServiceType, App.GetService<ILoginService>().ConnectionString())));

            //customer_DataServiceImplements.ForEach(ServiceType => services.AddSingleton(typeof(IDataService<Customer>), x => ActivatorUtilities.CreateInstance(x, ServiceType, App.GetService<ILoginService>().ConnectionString())));

            //order_DataServiceImplements.ForEach(ServiceType => services.AddSingleton(typeof(IDataService<Order>), x => ActivatorUtilities.CreateInstance(x, ServiceType, App.GetService<ILoginService>().ConnectionString())));

            //orderDetail_DataServiceImplements.ForEach(ServiceType => services.AddSingleton(typeof(IDataService<OrderDetail>), x => ActivatorUtilities.CreateInstance(x, ServiceType, App.GetService<ILoginService>().ConnectionString())));

            //product_DataServiceImplements.ForEach(ServiceType => services.AddSingleton(typeof(IDataService<Product>), x => ActivatorUtilities.CreateInstance(x, ServiceType, App.GetService<ILoginService>().ConnectionString())));

            //productType_DataServiceImplements.ForEach(ServiceType => services.AddSingleton(typeof(IDataService<ProductType>), x => ActivatorUtilities.CreateInstance(x, ServiceType, App.GetService<ILoginService>().ConnectionString())));

            // Domain Services
            services.AddSingleton(typeof(IDataService<Coupon>), x => ActivatorUtilities.CreateInstance(x, typeof(CouponDataService), GetService<ILoginService>().ConnectionString()));
            services.AddSingleton(typeof(IDataService<Customer>), x => ActivatorUtilities.CreateInstance(x, typeof(CustomerDataService), GetService<ILoginService>().ConnectionString()));
            services.AddSingleton(typeof(IDataService<Order>), x => ActivatorUtilities.CreateInstance(x, typeof(OrderDataService), GetService<ILoginService>().ConnectionString()));
            services.AddSingleton(typeof(IDataService<OrderDetail>), x => ActivatorUtilities.CreateInstance(x, typeof(OrderDetailDataService), GetService<ILoginService>().ConnectionString()));
            services.AddSingleton(typeof(IDataService<Product>), x => ActivatorUtilities.CreateInstance(x, typeof(ProductDataService), GetService<ILoginService>().ConnectionString()));
            services.AddSingleton(typeof(IDataService<ProductType>), x => ActivatorUtilities.CreateInstance(x, typeof(ProductType), GetService<ILoginService>().ConnectionString()));
            services.AddSingleton<IStatisticService, StatisticService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ReportViewModel>();
            services.AddTransient<ReportPage>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<ProductPage>();
            services.AddTransient<CategoryViewModel>();
            services.AddTransient<CategoryPage>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<OrderPage>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<StatisticViewModel>();
            services.AddTransient<StatisticPage>();
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<CustomerPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();
        UnhandledException += App_UnhandledException;
        AppDomain.CurrentDomain.UnhandledException += System_UnhandledException;
    }

    // Exceptions for App (Microsoft.UI.Xaml)
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO2: Log and handle exceptions as appropriate.
        // Update: this is still under improvement
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
        DisplayErrorDialog(e.Exception);
    }

    // System's exceptions
    private void System_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        DisplayErrorDialog(e.ExceptionObject);
    }

    // TODO2: Would rather log the error than displaying it (the App is not yet shown)
    private static void DisplayErrorDialog(object exObj)
    {
        //var errorMessage = string.Format((exObj as Exception)?.Message ?? "");
        //MessageDialog error = new(errorMessage, "An unhandled exception occurred");
        //error.Options = MessageDialogOptions.None;
        //_ = error.ShowAsync();
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await GetService<IActivationService>().ActivateAsync(args);

        // Eventhough the app loaded but there's still no XamlRoot, go to the ShellPage OnLoad event
        //await GetService<ILoginService>().Authenticate();
    }
}
