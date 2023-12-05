using CommunityToolkit.WinUI.UI.Controls;

using GroceryStore.MainApp.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp.Views;

public sealed partial class ProductPage : Page
{
    public ProductViewModel ViewModel
    {
        get;
    }

    public ProductPage()
    {
        ViewModel = App.GetService<ProductViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
