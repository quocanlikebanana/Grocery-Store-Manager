using GroceryStore.Domain.Model;
using GroceryStore.Domain.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Data.EntityFramework.Services
{
    public class CustomerDataService: IDataService<Customer>
    {
        private string _connectionString = string.Empty;

        public CustomerDataService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<int> Count()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) {
                int result = await context.Set<Customer>().CountAsync();
                return result;
            }
        }

        public async Task<Customer?> Create(Customer entity)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Customer>().AddAsync(entity);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                Customer? removeEntity = context.Set<Customer>().FirstOrDefault((e) => e.Id == id);
                if(removeEntity != null)
                {
                    context.Set<Customer>().Remove(removeEntity);
                }
                await context.SaveChangesAsync();
                return true;
            }
        }

        public Task<bool> Delete(int id1, int id2)
        {
            throw new NotImplementedException("!");
        }

        public Task<IEnumerable<Customer>> FilterDate(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> FilterDate(DateTime start, DateTime end, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> FilterPrice(double min, double max)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> FilterPrice(double min, double max, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer?> Get(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                Customer? result = await context.Set<Customer>().FirstOrDefaultAsync(e => e.Id == id);
                return result;
            }
        }

        public Task<Customer?> Get(int id1, int id2)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Customer> entities = await context.Set<Customer>().ToListAsync();
                return entities;
            }
        }

        public async Task<Result<Customer>> GetFull(string search = "", string sort = "", bool asc = true, object? lowerLimit = null, object? upperLimit = null, int perPage = 5, int pageNum = 1)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IQueryable<Customer> query = context.Set<Customer>();

                int totalCount = await query.CountAsync();
                int totalPage = (int)Math.Ceiling((double)totalCount / perPage);

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

                query = query.Skip((pageNum - 1) * perPage).Take(perPage);

                IEnumerable<Customer> entities = await query.ToListAsync();
                return new Result<Customer>(entities, totalPage);
            }
        }

        public async Task<Customer?> Update(int id, Customer entity)
        {
            entity.Id = id;
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                context.Set<Customer>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public  Task<Customer?> Update(int id1, int id2, Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
