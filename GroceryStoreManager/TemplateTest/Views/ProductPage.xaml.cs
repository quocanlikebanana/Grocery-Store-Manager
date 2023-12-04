using Microsoft.UI.Xaml.Controls;

using TemplateTest.ViewModels;

namespace TemplateTest.Views;

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
}
