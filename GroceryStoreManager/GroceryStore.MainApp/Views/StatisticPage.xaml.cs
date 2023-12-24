using GroceryStore.MainApp.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp.Views;

public sealed partial class StatisticPage : Page
{
    public StatisticViewModel ViewModel
    {
        get;
    }

    public StatisticPage()
    {
        ViewModel = App.GetService<StatisticViewModel>();
        InitializeComponent();
    }
}
