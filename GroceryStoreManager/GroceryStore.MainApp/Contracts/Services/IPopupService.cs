using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Contracts.Services;

public interface IPopupService
{
    event Action? OnWindowClose;
    void ShowWindow(object? content = null);
    void CloseWindow();
}

// TODO: sua ten OrderForm
