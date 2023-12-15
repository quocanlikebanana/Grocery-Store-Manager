using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.MainApp.Exceptions;

public enum LogicException
{
    None,
}

public class BussinessLogicException : Exception
{
    private readonly LogicException logicException;

    public BussinessLogicException(LogicException logicException)
    {
        this.logicException = logicException;
    }

    public LogicException LogicException => logicException;
}
