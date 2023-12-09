using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Service;

namespace GroceryStore.MainApp.Command.CRUD;

public abstract class CRUDCommandBase<T> : CommandBase where T : class
{
    protected readonly IDataService<T> _dataService;

    public CRUDCommandBase(IDataService<T> dataService) : base()
    {
        _dataService = dataService;
    }
}
