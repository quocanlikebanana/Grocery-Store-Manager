using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Command;

public class AsyncDelegateCommand : AsyncCommandBase
{
    private readonly Func<object?, bool> _canExecute;
    private readonly Func<object?, Task> _execute;

    public AsyncDelegateCommand(Func<object?, Task> execute, Action<Exception>? onException) : this(execute, s => true, onException)
    {
    }

    public AsyncDelegateCommand(Func<object?, Task> execute, Func<object?, bool> canExecute, Action<Exception>? onException) : base(onException)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        // REMEMBER: Prevent spamming
        return _canExecute(parameter);
    }

    protected async override Task ExecuteAsync(object? parameter)
    {
        await _execute(parameter);
    }
}


public class AsyncDelegateCommand<T> : AsyncCommandBase where T : class
{
    private readonly Func<object?, bool> _canExecute;
    private readonly Func<T?, Task> _execute;

    public AsyncDelegateCommand(Func<T?, Task> execute, Action<Exception>? onException) : this(execute, s => true, onException)
    {
    }

    public AsyncDelegateCommand(Func<T?, Task> execute, Func<object?, bool> canExecute, Action<Exception>? onException) : base(onException)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        // REMEMBER: Prevent spamming
        return _canExecute(parameter);
    }

    protected async override Task ExecuteAsync(object? parameter)
    {
        await _execute(parameter as T);
    }
}
