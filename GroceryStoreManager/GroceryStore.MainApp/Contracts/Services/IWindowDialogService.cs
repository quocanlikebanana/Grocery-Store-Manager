using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Contracts.Services;

public interface IWindowDialogService
{
    void ShowWindow();
    void CloseWindow();
}
