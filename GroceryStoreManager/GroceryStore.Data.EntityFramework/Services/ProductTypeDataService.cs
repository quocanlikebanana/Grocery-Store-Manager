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
    public class ProductTypeDataService : IDataService<ProductType>
    {
        private string _connectionString = string.Empty;

        public ProductTypeDataService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<ProductType?> Create(ProductType entity)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<ProductType>().AddAsync(entity);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                ProductType? removeEntity = context.Set<ProductType>().FirstOrDefault((e) => e.Id == id);
                if (removeEntity != null)
                {
                    context.Set<ProductType>().Remove(removeEntity);
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

        public async Task<ProductType?> Get(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                ProductType? result = await context.Set<ProductType>().FirstOrDefaultAsync(e => e.Id == id);
                return result;
            }
        }

        public Task<ProductType?> Get(int id1, int id2)
        {
            throw new NotImplementedException("!");
        }

        public async Task<IEnumerable<ProductType>> GetAll()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<ProductType> entities = await context.Set<ProductType>().ToListAsync();
                return entities;
            }
        }

        public Task<IEnumerable<ProductType>> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductType?> Update(int id, ProductType entity)
        {
            entity.Id = id;
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                context.Set<ProductType>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public Task<ProductType?> Update(int id1, int id2, ProductType entity)
        {
            throw new NotImplementedException("!");
        }
    }
}
