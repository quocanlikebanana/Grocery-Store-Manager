using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Data.EntityFramework.Services;

public class OrderDataService : IDataService<Order>
{
    private readonly string _connectionString = string.Empty;

    public OrderDataService(string connectionString)
    {
        this._connectionString = connectionString;
    }
    public async Task<Order?> Create(Order entity)
    {
        using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
        {
            var result = await context.Set<Order>().AddAsync(entity);
            await context.SaveChangesAsync();
            return result.Entity;
        }
    }

    public async Task<bool> Delete(int id)
    {
        using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
        {
            Order? removeEntity = await context.Set<Order>().FirstOrDefaultAsync(o => o.Id == id);
            if (removeEntity != null)
            {
                context.Set<Order>().Remove(removeEntity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }

    public Task<bool> Delete(int id1, int id2)
    {
        throw new NotImplementedException();
    }

    public async Task<Order?> Get(int id)
    {
        using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
        {
            Order? order = await context.Set<Order>().Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            return order;
        }
    }

    public Task<Order?> Get(int id1, int id2)
    {
        throw new NotImplementedException();
    }

        public async Task<IEnumerable<Order>> GetAll()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) {
                IEnumerable<Order> entities = await context.Set<Order>().Include(o => o.Customer).ToListAsync();
                return entities;
            }
        }

        public Task<IEnumerable<Order>> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> Update(int id, Order entity)
        {
            entity.Id = id;
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                context.Set<Order>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

    public Task<Order?> Update(int id1, int id2, Order entity)
    {
        throw new NotImplementedException();
    }
}
