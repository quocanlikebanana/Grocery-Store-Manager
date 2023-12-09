using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.WinUI.Deferred;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Control;
using GroceryStore.MainApp.ViewModels.SubWindowVM;
using GroceryStore.MainApp.Views.SubWindowView;
using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace GroceryStore.MainApp.Services;


public class FormDialogService : IWindowDialogService
{
    private readonly Type _tView;
    private readonly Func<IWindowDialogService, FormVMBase> _createFormVM;

    private Window? _window;

    public FormDialogService(Type createTView, Func<IWindowDialogService, FormVMBase> createFormVM)
    {
        _tView = createTView;
        _createFormVM = createFormVM;
    }

    public void CloseWindow()
    {
        _window?.Close();
    }

    public void ShowWindow()
    {
        _window = new Subwindow();
        var view = Activator.CreateInstance(_tView) as FrameworkElement;
        if (view == null)
        {
            return;
        }
        var viewModel = _createFormVM.Invoke(this);
        view.DataContext = viewModel;
        _window.Content = view;
        _window.Show();
    }
}
