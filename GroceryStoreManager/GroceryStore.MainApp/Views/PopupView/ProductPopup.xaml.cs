using DevExpress.WinUI.Editors;
using DevExpress.WinUI.Grid;
using GroceryStore.Domain.Model;
using GroceryStore.MainApp.ControlHelper;
using GroceryStore.MainApp.Decorators;
using GroceryStore.MainApp.Strategies;
using GroceryStore.MainApp.ViewModels.PopupVM;
using Microsoft.UI.Xaml.Controls;
using System.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GroceryStore.MainApp.Views.PopupView;

public sealed partial class ProductPopup : Page
{
    private readonly AutoSuggestBoxHandler<ProductType> _productTypeAutoSuggestBoxHandler;

    public ProductPopup(PopupVMBase formVM)
    {
        ViewModel = (ProductPopupVM)formVM;
        InitializeComponent();

        _productTypeAutoSuggestBoxHandler = new(new SearchByUniqueString<ProductType>(ViewModel.AvailibleProductType, ProductTypeDecorator.ToSearchString), OnSelectedProductType, ProductTypeDecorator.ToDisplayString, null);
        _productTypeAutoSuggestBoxHandler.Assign(ProductTypeASB, ViewModel.SelectedProductType);

        var currencyControls = new List<SpinEdit>()
        {
            Price_D_SE,
            BasePrice_D_SE,
        };
        foreach (var se in currencyControls)
        {
            se.TextInputSettings = new TextInputMaskSettings()
            {
                Mask = "c0",
                MaskType = MaskType.Numeric,
                MaskCulture = new CultureInfo("vi-VN")
                {
                    NumberFormat = new NumberFormatInfo()
                    {
                        CurrencySymbol = "đ",
                        CurrencyDecimalDigits = 0,
                        CurrencyGroupSeparator = ",",
                        CurrencyPositivePattern = 3,
                    },
                },
            };
        }
    }

    public ProductPopupVM ViewModel
    {
        get; private set;
    }

    private void OnSelectedProductType(ProductType? selected)
    {
        ViewModel.SelectedProductType = selected;
    }
}
