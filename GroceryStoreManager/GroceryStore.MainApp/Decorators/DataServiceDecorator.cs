using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;

namespace GroceryStore.MainApp.Decorators;

public class DataServiceDecorator<T>
{
    private readonly IDataService<T> _dataService;

    public DataServiceDecorator(IDataService<T> dataService)
    {
        _dataService = dataService;
    }

    public async Task<IEnumerable<T>> GetFull(string sortColumn, string searchKey, ,int perPage, int pageNum)
    {
        var list = new List<T>();
        return list;
    }

}
