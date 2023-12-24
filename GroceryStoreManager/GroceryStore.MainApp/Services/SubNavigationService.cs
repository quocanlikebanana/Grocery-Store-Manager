using DevExpress.Mvvm;
using GroceryStore.MainApp.Stores;
using System.ComponentModel;

namespace GroceryStore.MainApp.Services;

public class SubNavigationService
{
    private readonly SubNavigationStore _navigationStore;
    private readonly Func<INotifyPropertyChanged> _createVM;

    public SubNavigationService(SubNavigationStore navigationStore, Func<INotifyPropertyChanged> createVM)
    {
        _navigationStore = navigationStore;
        _createVM = createVM;
    }

    public void Navigate()
    {
        _navigationStore.CurrentVM = _createVM.Invoke();
    }
}
