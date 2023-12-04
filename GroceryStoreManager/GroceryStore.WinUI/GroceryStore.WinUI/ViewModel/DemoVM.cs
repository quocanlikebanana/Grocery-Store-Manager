using DevExpress.Mvvm;
using GroceryStore.WinUI.DemoModel;
using System.Collections.ObjectModel;

namespace GroceryStore.WinUI.ViewModel
{
    public class DemoVM : ViewModelBase
    {
        public DemoVM()
        {
            Source = ProductsDataModel.GetProducts();
        }
        public ObservableCollection<Product> Source { get; }
    }
}
