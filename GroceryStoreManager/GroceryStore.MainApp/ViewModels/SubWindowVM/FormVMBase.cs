﻿using System;
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

public enum FormResult
{
    Accept,
    Indeterminate,
}

public abstract class FormVMBase : ObservableRecipient
{
    private readonly IWindowDialogService _dialogService;

    protected FormResult _result;
    public FormResult Result
    {
        get;
    }

    protected FormVMBase(IWindowDialogService dialogService)
    {
        _result = FormResult.Indeterminate;
        _dialogService = dialogService;

        AcceptCommand = new DelegateCommand(Accept);
        DeclineCommand = new DelegateCommand(Decline);
    }

    public ICommand AcceptCommand
    {
        get;
    }
    public ICommand DeclineCommand
    {
        get;
    }


    public void Accept(object? obj)
    {
        var data = GetFormData();
        var result = OnAccept(data);
        if (result == true)
        {
            _result = FormResult.Accept;
            _dialogService?.CloseWindow();
        }
        else
        {
            _result = FormResult.Indeterminate;
            OnInvalid();
        }
    }

    public void Decline(object? obj)
    {
        _dialogService?.CloseWindow();
    }

    protected virtual bool OnAccept(object formData)
    {
        return true;
    }

    protected virtual void OnInvalid()
    {
    }

    public abstract object GetFormData();
}
