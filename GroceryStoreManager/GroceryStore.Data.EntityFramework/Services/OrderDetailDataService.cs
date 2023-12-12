using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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

        public async Task<int> Count()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<OrderDetail>().CountAsync();
                return result;
            }
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

        public async Task<bool> Delete(int orderID, int proID)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) {
                OrderDetail? removeEntity = await context.Set<OrderDetail>().FirstOrDefaultAsync(od => od.Order != null && od.Order.Id == orderID && od.Product != null && od.Product.Id == proID);
                if(removeEntity != null)
                {
                    context.Set<OrderDetail>().Remove(removeEntity);
                    await context.SaveChangesAsync();   
                    return true;
                } else { return false; }
            }
        }

        public Task<IEnumerable<OrderDetail>> FilterDate(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetail>> FilterDate(DateTime start, DateTime end, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetail>> FilterPrice(double min, double max)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetail>> FilterPrice(double min, double max, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail?> Get(int id) 
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetail?> Get(int orderID, int proID)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) { 
                OrderDetail? orderDetail = await context.Set<OrderDetail>().Include(o => o.Order).Include(o=> o.Product).ThenInclude(p => p != null ? p.Type : null).FirstOrDefaultAsync(od => od.Order != null && od.Order.Id == orderID && od.Product != null && od.Product.Id == proID);
                return orderDetail;
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAll()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) {
                IEnumerable<OrderDetail> orderDetails = await context.Set<OrderDetail>().Include(o => o.Order).Include(o => o.Product).ThenInclude(p => p != null ? p.Type : null).ToListAsync();
                return orderDetails;
            }
        }

      

        public Task<OrderDetail?> Update(int id, OrderDetail entity)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetail?> Update(int orderID, int proID, OrderDetail entity)
        {
            if (entity.Order != null && entity.Product != null) { 
                entity.Order.Id = orderID;
                entity.Product.Id = proID;
                using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
                {
                    context.Set<OrderDetail>().Update(entity);
                    await context.SaveChangesAsync();
                    return entity;
                }
            } else
            {
                return null;
            }
        }
    }
}
