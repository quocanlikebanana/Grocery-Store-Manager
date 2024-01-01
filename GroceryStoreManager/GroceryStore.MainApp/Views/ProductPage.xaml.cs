using DevExpress.WinUI.Grid;
using GroceryStore.MainApp.ViewModels;
using GroceryStore.MainApp.ViewModels.SubVM;
using GroceryStore.MainApp.Views.SubView;
using Microsoft.UI.Xaml.Controls;
using System.Globalization;

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

        var currencyControls = new List<GridMaskColumn>()
        {
            Price_GMC,
            BasePrice_GMC,
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
            var vm = new DetailProductVM(id);
            Frame.Navigate(typeof(DetailProductPage), vm);
        };
    }
}
