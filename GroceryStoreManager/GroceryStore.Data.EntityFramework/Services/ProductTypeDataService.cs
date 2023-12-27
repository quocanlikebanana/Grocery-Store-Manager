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

        public async Task<int> Count()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<ProductType>().CountAsync();
                return result;
            }
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

        public Task<IEnumerable<ProductType>> FilterDate(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductType>> FilterDate(DateTime start, DateTime end, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductType>> FilterPrice(double min, double max)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductType>> FilterPrice(double min, double max, int page, int perPage)
        {
            throw new NotImplementedException();
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

        public async Task<Result<ProductType>> GetFull(string search = "", string sort = "", bool asc = true, object? lowerLimit = null, object? upperLimit = null, int perPage = 5, int pageNum = 1)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IQueryable<ProductType> query = context.Set<ProductType>();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.Name.Contains(search));
                }

                if (!string.IsNullOrEmpty(sort))
                {
                    if (asc)
                    {
                        query = query.OrderBy(c => EF.Property<object>(c, sort));
                    }
                    else
                    {
                        query = query.OrderByDescending(c => EF.Property<object>(c, sort));
                    }
                }

                int totalCount = await query.CountAsync();
                int totalPage = (int)Math.Ceiling((double)totalCount / perPage);

                query = query.Skip((pageNum - 1) * perPage).Take(perPage);

                IEnumerable<ProductType> entities = await query.ToListAsync();
                return new Result<ProductType>(entities, totalPage);
            }
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
