using Microsoft.UI.Xaml.Controls;

using TemplateTest.ViewModels;

namespace TemplateTest.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
