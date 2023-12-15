using DevExpress.WinUI.Editors;
using GroceryStore.Domain.Model;
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
    private readonly AutoSuggestBoxHandler<Customer> _customerAutoSuggestBoxHandler;
    private readonly AutoSuggestBoxHandler<Product> _productAutoSuggestBoxHandler;

    public OrderPopup(PopupVMBase formVM)
    {
        ViewModel = (OrderPopupVM)formVM;
        InitializeComponent();

        _customerAutoSuggestBoxHandler = new(new SearchByUniqueString<Customer>(ViewModel.AvailibleCustomer, CustomerDecorator.ToSearchString), OnSelectedCustomer, CustomerDecorator.ToDisplayString, null);
        _customerAutoSuggestBoxHandler.Assign(CustomerASB, ViewModel.SelectedCustomer);

        _productAutoSuggestBoxHandler = new(new SearchByUniqueString<Product>(ViewModel.AvailbleProduct, ProductDecorator.ToSearchString), OnSelectedProduct, ProductDecorator.ToDisplayString, null);
        _productAutoSuggestBoxHandler.Assign(ProductASB, ViewModel.SelectedProduct);
    }

    public OrderPopupVM ViewModel
    {
        get; private set;
    }

    private void OnSelectedCustomer(Customer? selected)
    {
        ViewModel.SelectedCustomer = selected;
    }

    private void OnSelectedProduct(Product? selected)
    {
        ViewModel.SelectedProduct = selected;
    }

    private void SpineditIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
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
