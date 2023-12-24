using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Service
{
    public class Result<T>
    {
        IEnumerable<T> Items { get; }
        int TotalPage { get; }
        public Result(IEnumerable<T> items, int totalPage)
        {
            Items = items;
            TotalPage = totalPage;
        }
    }

    public interface IDataService<T>
    {
        // Basic 
        Task<IEnumerable<T>> GetAll();
        Task<T?> Get(int id);
        Task<T?> Get(int id1,int id2);
        Task<T?>  Create(T entity);
        Task<T?> Update(int id,T entity);
        Task<T?> Update(int id1,int id2,T entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(int id1,int id2);

        // Filter + paging
        Task<Result<T>> GetFull(string search = "", string sort = "", bool asc = true, object? lowerLimit = null, object? upperLimit = null, int perPage = 5, int pageNum = 1);
        Task<IEnumerable<T>> FilterPrice(double min, double max);
        Task<IEnumerable<T>> FilterPrice(double min, double max, int page, int perPage);

        Task<IEnumerable<T>> FilterDate(DateTime start, DateTime end);
        Task<IEnumerable<T>> FilterDate(DateTime start, DateTime end, int page, int perPage);


        // Statictics
        Task<int> Count();

    }
}
