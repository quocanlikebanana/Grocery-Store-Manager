using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Service;

public interface IDataService<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetAll(int id);
    Task<T?> Get(int id);
    Task<T?> Get(int id1,int id2);
    Task<T?>  Create(T entity);
    Task<T?> Update(int id,T entity);
    Task<T?> Update(int id1,int id2,T entity);
    Task<bool> Delete(int id);
    Task<bool> Delete(int id1,int id2);
}
