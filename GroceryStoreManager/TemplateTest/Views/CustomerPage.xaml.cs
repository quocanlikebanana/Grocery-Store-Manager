using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using TemplateTest.ViewModels;

namespace TemplateTest.Views;

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

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
