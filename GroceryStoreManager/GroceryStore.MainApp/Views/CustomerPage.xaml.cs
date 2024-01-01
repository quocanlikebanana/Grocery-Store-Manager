using CommunityToolkit.WinUI.UI.Controls;

using GroceryStore.MainApp.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp.Views;

public sealed partial class CustomerPage : Page
{
    public CustomerViewModel ViewModel
    {
        get;
    }

    public CustomerPage()
    {
        ViewModel = App.GetService<CustomerViewModel>();
        InitializeComponent();
    }
}
