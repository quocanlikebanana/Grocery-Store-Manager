using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GroceryStore.MainApp.Command;

public class DelegateCommand : CommandBase
{
    private readonly Func<object?, bool> _canExecute;
    private readonly Action<object?> _execute;

    public DelegateCommand(Action<object?> execute) : this(execute, s => true)
    {
    }

    public DelegateCommand(Action<object?> execute, Func<object?, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        return _canExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        _execute(parameter);
    }
}

public class DelegateCommand<T> : CommandBase where T : class
{
    private readonly Func<object?, bool> _canExecute;
    private readonly Action<T?> _execute;

    public DelegateCommand(Action<T?> execute) : this(execute, s => true)
    {
    }

    public DelegateCommand(Action<T?> execute, Func<object?, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        return _canExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        _execute(parameter as T);
    }
}
