using GroceryStore.MainApp.ViewModels.SubVM;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GroceryStore.MainApp.Views.SubView;

public sealed partial class DetailOrderPage : Page
{
    public DetailOrderVM? ViewModel { get; private set; }

    public DetailOrderPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel = (DetailOrderVM)e.Parameter;
        ViewModel.GoBackHandle = () =>
        {
            //Frame.Navigate(typeof(OrderPage));
            Frame.GoBack();
        };
        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        ViewModel = null;
        base.OnNavigatedFrom(e);
    }
}
