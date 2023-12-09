using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GroceryStore.MainApp.Command;

public abstract class AsyncCommandBase : ICommand
{
    private readonly Action<Exception>? _onException;
    public event EventHandler? CanExecuteChanged;

    private bool _isExecuting;
    public bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            _isExecuting = value;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }

    public AsyncCommandBase(Action<Exception>? onException)
    {
        _onException = onException;
    }

    public virtual bool CanExecute(object? parameter)
    {
        // Prevent spamming
        return !IsExecuting;
    }

    public async void Execute(object? parameter)
    {
        IsExecuting = true;
        try
        {
            await ExecuteAsync(parameter);
        }
        catch (Exception ex)
        {
            _onException?.Invoke(ex);
        }
        IsExecuting = false;
    }

    protected abstract Task ExecuteAsync(object? parameter);
}
