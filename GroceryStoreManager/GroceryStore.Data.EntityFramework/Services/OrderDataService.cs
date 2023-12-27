﻿using Azure;
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
    public class OrderDataService : IDataService<Order>
    {
        private string _connectionString = string.Empty;

        public OrderDataService(string connectionString) {
            this._connectionString = connectionString;
        }

        public async Task<int> Count()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                int result = await context.Set<Order>().CountAsync();
                return result;
            }
        }

        public async Task<Order?> Create(Order entity)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) { 
                var result = await context.Set<Order>().AddAsync(entity);
                await context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) { 
                Order? removeEntity = await context.Set<Order>().FirstOrDefaultAsync(o => o.Id == id);
                if(removeEntity != null)
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

        public async Task<IEnumerable<Order>> FilterDate(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Order> entities = await context.Set<Order>().Include(o => o.Customer).Include(o =>o.details).ThenInclude(d => d.Product).ThenInclude(d => d != null ? d.Type : null).Where(o => o.OrderDate >= start && o.OrderDate <= end).ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Order>> FilterDate(DateTime start, DateTime end, int page, int perPage)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Order> entities = await context.Set<Order>().Include(o => o.Customer).Include(o => o.details).ThenInclude(d => d.Product).ThenInclude(d => d != null ? d.Type : null).Where(o => o.OrderDate >= start && o.OrderDate <= end).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                return entities;
            }
        }

        public Task<IEnumerable<Order>> FilterPrice(double min, double max)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> FilterPrice(double min, double max, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> Get(int id)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString)) { 
                Order? order = await context.Set<Order>().Include(o => o.Customer).Include(o => o.details).ThenInclude(d => d.Product).ThenInclude(d => d != null? d.Type : null).FirstOrDefaultAsync(o => o.Id == id);
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
                IEnumerable<Order> entities = await context.Set<Order>().Include(o => o.Customer).Include(o => o.details).ThenInclude(d => d.Product).ThenInclude(d => d != null ? d.Type : null).ToListAsync();
                return entities;
            }
        }

        public async Task<Result<Order>> GetFull(string search = "", string sort = "", bool asc = true, object? lowerLimit = null, object? upperLimit = null, int perPage = 5, int pageNum = 1)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IQueryable<Order> query = context.Set<Order>();

                query = query.Include(o => o.Customer).Include(o => o.details).ThenInclude(d => d.Product).ThenInclude(d => d != null ? d.Type : null);

                query = query.Where(o => o.Customer != null);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(o => o.Customer.Name.Contains(search));
                }

                if (lowerLimit != null && upperLimit != null)
                {
                    query = query.Where(o => o.OrderDate >= (DateTime)lowerLimit && o.OrderDate <= (DateTime)upperLimit);
                }

                if (!string.IsNullOrEmpty(sort))
                {
                    if (asc)
                    {
                        query = query.OrderBy(o => EF.Property<object>(o, sort));
                    }
                    else
                    {
                        query = query.OrderByDescending(o => EF.Property<object>(o, sort));
                    }
                }
                
                int totalCount = await query.CountAsync();
                int totalPage = (int)Math.Ceiling((double)totalCount / perPage);

                query = query.Skip((pageNum - 1) * perPage).Take(perPage);

                IEnumerable<Order> entities = await query.ToListAsync();
                return new Result<Order>(entities, totalPage);
            }
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
}
