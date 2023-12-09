using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Service;

namespace GroceryStore.MainApp.Command.CRUD;

public class CreateCommand<T> : CRUDCommandBase<T> where T : class
{
    public CreateCommand(IDataService<T> dataService) : base(dataService)
    {
    }
    
    // Function Injection


    public override void Execute(object? parameter)
    {

    }
}
