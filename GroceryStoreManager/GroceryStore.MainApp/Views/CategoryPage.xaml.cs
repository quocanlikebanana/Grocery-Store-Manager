using GroceryStore.MainApp.ViewModels;
using GroceryStore.MainApp.ViewModels.CategoryVMs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GroceryStore.MainApp.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class CategoryPage : Page
{
    public CategoryViewModel ViewModel
    {
        get;
    }

    public CategoryPage()
    {
        ViewModel = App.GetService<CategoryViewModel>();
        InitializeComponent();
    }
}

public class FormDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? OnNull { get; set; }
    public DataTemplate? Form { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        var vm = item as FormVM;
        if (vm != null && vm.IsBase == false)
        {
            return Form!;
        }
        return OnNull!;
    } 
}
