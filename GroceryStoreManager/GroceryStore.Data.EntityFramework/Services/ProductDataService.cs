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
    public class ProductDataService : IDataService<Product>
    {
        private string _connectionString = string.Empty;

        public ProductDataService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<int> Count()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Product>().CountAsync();
                return result;
            }
        }

        public async Task<Product?> Create(Product entity)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Product>().AddAsync(entity);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                Product? removeEntity = context.Set<Product>().FirstOrDefault((e) => e.Id == id);
                if (removeEntity != null)
                {
                    context.Set<Product>().Remove(removeEntity);
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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> FilterDate(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> FilterDate(DateTime start, DateTime end, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> FilterPrice(double min, double max)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) {
                IEnumerable<Product> entities = await context.Set<Product>().Include(x => x.Type).Where(x => x.Price >= min && x.Price <= max).ToListAsync();    
                return entities;
            }
        }

        public async Task<IEnumerable<Product>> FilterPrice(double min, double max, int page, int perPage)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Product> entities = await context.Set<Product>().Include(x => x.Type).Where(x => x.Price >= min && x.Price <= max).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                return entities;
            }
        }

        public async Task<Product?> Get(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                Product? result = await context.Set<Product>().Include(t => t.Type).FirstOrDefaultAsync(e => e.Id == id);
                return result;
            }
        }

        public Task<Product?> Get(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Product> entities = await context.Set<Product>().Include(p=> p.Type).ToListAsync();
                return entities;
            }
        }

        public async Task<Result<Product>> GetFull(string search = "", string sort = "", bool asc = true, object? lowerLimit = null, object? upperLimit = null, int perPage = 5, int pageNum = 1)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IQueryable<Product> query = context.Set<Product>();

                int totalCount = await query.CountAsync();
                int totalPage = (int)Math.Ceiling((double)totalCount / perPage);

                query = query.Include(p => p.Type);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p => p.Name.Contains(search));
                }

                if (lowerLimit != null && upperLimit != null)
                {
                    query = query.Where(p => p.Price >= (double)lowerLimit && p.Price <= (double)upperLimit);
                }

                if (!string.IsNullOrEmpty(sort))
                {
                    if (asc)
                    {
                        query = query.OrderBy(p => EF.Property<object>(p, sort));
                    }
                    else
                    {
                        query = query.OrderByDescending(p => EF.Property<object>(p, sort));
                    }
                }

                query = query.Skip((pageNum - 1) * perPage).Take(perPage);

                IEnumerable<Product> entities = await query.ToListAsync();
                return new Result<Product>(entities, totalPage);
            }
        }

        public async Task<Product?> Update(int id, Product entity)
        {
            entity.Id = id;
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                context.Set<Product>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public Task<Product?> Update(int id1, int id2, Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
