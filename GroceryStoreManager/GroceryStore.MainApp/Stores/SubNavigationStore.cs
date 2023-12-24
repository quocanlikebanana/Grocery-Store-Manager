using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Stores;

public class SubNavigationStore
{
    public event Action? CurrentVMChanged;

    public SubNavigationStore()
    {
    }

    private INotifyPropertyChanged? _currentVM;

    public INotifyPropertyChanged? CurrentVM
    {
        get { return _currentVM; }
        set
        {
            _currentVM = value;
            CurrentVMChanged?.Invoke();
        }
    }
}
