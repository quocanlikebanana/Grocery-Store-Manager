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

namespace GroceryStore.MainApp.ViewModels.PopupVM;

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
        var check = OnAccepting(null);
        if (check)
        {
            Result = PopupResult.Suceed;
            _dialogService?.CloseWindow();
        }
        else
        {
            Result = PopupResult.Failed;
            OnInvalid();
        }
    }

    public void Decline(object? obj)
    {
        Result = PopupResult.Canceled;
        _dialogService?.CloseWindow();
    }

    public abstract object? GetFormData();

    protected virtual bool OnAccepting(object? formData)
    {
        return true;
    }

    protected virtual void OnInvalid()
    {
    }
}
