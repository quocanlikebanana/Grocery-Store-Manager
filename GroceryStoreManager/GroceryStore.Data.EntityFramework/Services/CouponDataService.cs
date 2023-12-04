using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GroceryStore.Data.EntityFramework.Services
{
    public class CouponDataService : IDataService<Coupon>
    {

        private string _connectionString = string.Empty;

        public CouponDataService(string connectionString) {
            this._connectionString = connectionString;
        }

        public async Task<Coupon?> Create(Coupon entity)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Coupon>().AddAsync(entity);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                Coupon? removeEntity = context.Set<Coupon>().FirstOrDefault((e) => e.Id == id);
                if (removeEntity != null)
                {
                    context.Set<Coupon>().Remove(removeEntity);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public Task<bool> Delete(int id1, int id2)
        {
            throw new NotImplementedException("!");
        }

        public async Task<Coupon?> Get(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                Coupon? result = await context.Set<Coupon>().FirstOrDefaultAsync(e => e.Id == id);
                return result;
            }
        }

        public  Task<Coupon?> Get(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Coupon>> GetAll()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Coupon> entities = await context.Set<Coupon>().ToListAsync();
                return entities;
            }
        }

        public async Task<Coupon?> Update(int id, Coupon entity)
        {
            entity.Id = id;
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                context.Set<Coupon>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public Task<Coupon?> Update(int id1, int id2, Coupon entity)
        {
            throw new NotImplementedException();
        }
    }
}
