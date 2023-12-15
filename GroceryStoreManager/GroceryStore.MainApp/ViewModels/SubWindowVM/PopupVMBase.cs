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

    public void Accept(object? obj)
    {
        var data = GetFormData();
        var result = ContinueAccept(data);
        if (result == true)
        {
            _dialogService?.CloseWindow();
        }
        else
        {
            OnInvalid();
        }
    }

    public void Decline(object? obj)
    {
        _dialogService?.CloseWindow();
    }

    protected virtual bool ContinueAccept(object formData)
    {
        return true;
    }

    protected abstract void OnInvalid();

    public abstract object GetFormData();
}
