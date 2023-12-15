using DevExpress.WinUI.Editors;
using GroceryStore.MainApp.ControlHelper;
using GroceryStore.MainApp.Decorators;
using GroceryStore.MainApp.Strategies;
using GroceryStore.MainApp.ViewModels.SubWindowVM;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GroceryStore.MainApp.Views.PopupView;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class OrderPopup : Page
{
    private readonly AutoSuggestBoxHandler<DecASBCustomer> customerAutoSuggestBoxHandler;
    private readonly AutoSuggestBoxHandler<DecASBProduct> productAutoSuggestBoxHandler;

    public OrderPopup(PopupVMBase formVM)
    {
        ViewModel = (OrderPopupVM)formVM;
        InitializeComponent();

        var availibleCustomerASB = ViewModel.AvailibleCustomer.Select(x => new DecASBCustomer(x)).ToList();
        var availibleProductASB = ViewModel.AvailbleProduct.Select(x => new DecASBProduct(x)).ToList();

        customerAutoSuggestBoxHandler = new(new SearchByUniqueString<DecASBCustomer>(availibleCustomerASB, DecASBCustomer.ToUniqueString), OnSelectedCustomer, DecASBCustomer.ToDisplayString, null);
        customerAutoSuggestBoxHandler.Assign(CustomerASB);

        productAutoSuggestBoxHandler = new(new SearchByUniqueString<DecASBProduct>(availibleProductASB, DecASBProduct.ToUniqueString), OnSelectedProduct, DecASBProduct.ToDisplayString, null);
        productAutoSuggestBoxHandler.Assign(ProductASB);
    }

    public OrderPopupVM ViewModel
    {
        get; private set;
    }

    private void OnSelectedCustomer(DecASBCustomer? selected)
    {
        ViewModel.SelectedCustomer = selected?.Get();
    }

    private void OnSelectedProduct(DecASBProduct? selected)
    {
        ViewModel.SelectedProduct = selected?.Get();
    }

    private void CouponIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // SpinEdit : TextEdit
        if (sender is not TextEdit textEdit)
        {
            return;
        }
        if ((bool)e.NewValue == false)
        {
            textEdit.EditValue = 0.0;
        }
    }
}
