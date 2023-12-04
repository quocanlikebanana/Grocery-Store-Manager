using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Data.EntityFramework.Services
{
    public class OrderDetailDataService : IDataService<OrderDetail>
    {
        private string _connectionString = string.Empty;

        public OrderDetailDataService(string connectionString) {
            this._connectionString = connectionString;
        }
        public async Task<OrderDetail?> Create(OrderDetail entity)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) { 
                var result = await context.Set<OrderDetail>().AddAsync(entity);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int  orderID, int proID)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) {
                OrderDetail? removeEntity = await context.Set<OrderDetail>().FirstOrDefaultAsync(od => od.order.Id == orderID && od.product.Id == proID);
                if(removeEntity != null)
                {
                    context.Set<OrderDetail>().Remove(removeEntity);
                    await context.SaveChangesAsync();   
                    return true;
                } else { return false; }
            }
        }

        public Task<OrderDetail?> Get(int id) 
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetail?> Get(int orderID, int proID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetail>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail?> Update(int id, OrderDetail entity)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail?> Update(int id1, int id2, OrderDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
