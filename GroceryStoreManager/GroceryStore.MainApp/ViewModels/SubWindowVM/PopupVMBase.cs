using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using GroceryStore.MainApp.Command;
using GroceryStore.MainApp.Contracts.Services;
using GroceryStore.MainApp.Contracts.ViewModels;

namespace GroceryStore.MainApp.ViewModels.SubWindowVM;

public enum PopupResult
{
    None,
    Failed,
    Canceled,
    Suceed,
}

public abstract class PopupVMBase : ObservableRecipient
{
    private readonly IPopupService _dialogService;
    protected object? _content = null;

    protected PopupVMBase(IPopupService dialogService, object? content = null)
    {
        _dialogService = dialogService;

        AcceptCommand = new DelegateCommand(Accept);
        DeclineCommand = new DelegateCommand(Decline);
        _content = content;
    }

    public ICommand AcceptCommand { get; }
    public ICommand DeclineCommand { get; }

    public PopupResult Result { get; private set; } = PopupResult.None;

    // When data is validated, perform submit to the system
    public void Accept(object? obj)
    {
        var data = GetFormData();
        var check = AcceptResultCheck(data);
        Result = check ? PopupResult.Suceed : PopupResult.Failed;
        _dialogService?.CloseWindow();
    }

    public void Decline(object? obj)
    {
        Result = PopupResult.Canceled;
        _dialogService?.CloseWindow();
    }

    public abstract object GetFormData();

    protected virtual bool AcceptResultCheck(object formData)
    {
        return true;
    }
}
