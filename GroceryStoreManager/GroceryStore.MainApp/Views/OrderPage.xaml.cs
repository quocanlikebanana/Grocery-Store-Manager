using System.Globalization;
using DevExpress.WinUI.Grid;
using GroceryStore.MainApp.Factories;
using GroceryStore.MainApp.ViewModels;
using GroceryStore.MainApp.ViewModels.SubVM;
using GroceryStore.MainApp.Views.SubView;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace GroceryStore.MainApp.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class OrderPage : Page
{
    public OrderViewModel ViewModel
    {
        get;
    }

    public OrderPage()
    {
        ViewModel = App.GetService<OrderViewModel>();
        InitializeComponent();

        // https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberformatinfo.currencypositivepattern?view=net-8.0&redirectedfrom=MSDN#System_Globalization_NumberFormatInfo_CurrencyPositivePattern
        var currencyControls = new List<GridMaskColumn>()
        {
            TotalPrice_GMC,
            TotalDiscount_GMC,
        };
        foreach (var gmc in currencyControls)
        {
            gmc.MaskCulture = new CultureInfo("vi-VN")
            {
                NumberFormat = new NumberFormatInfo()
                {
                    CurrencySymbol = "đ",
                    CurrencyDecimalDigits = 0,
                    CurrencyGroupSeparator = ",",
                    CurrencyPositivePattern = 3,
                },
            };
        }

        ViewModel.GoToDetail = (id) =>
        {
            var vm = new DetailOrderVM(id);
            Frame.Navigate(typeof(DetailOrderPage), vm);
        };
    }
}
