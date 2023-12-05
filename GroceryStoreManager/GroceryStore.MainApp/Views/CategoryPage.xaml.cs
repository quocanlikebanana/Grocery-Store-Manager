using GroceryStore.MainApp.ViewModels;

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
