﻿using GroceryStore.Domain.Model;
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
    public class StatisticService : IStatisticService
    {
        private string _connectionString = string.Empty;

        public StatisticService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<double> GetTotalRevenue()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Order>().SumAsync(o => o.TotalPrice);
                return result;
            }
        }

        public async Task<double> GetTotalRevenue(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Order>().Where(o => o.OrderDate >= start && o.OrderDate <= end).SumAsync(o => o.TotalPrice);
                return result;
            }
        }

        public async Task<double> GetAverageRevenue()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalRevenue = await GetTotalRevenue();
                var numberOfOrders = await GetNumberOf();
                var result = totalRevenue / numberOfOrders;
                return result;
            }
        }

        public async Task<double> GetAverageRevenue(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalRevenue = await GetTotalRevenue(start, end);
                var numberOfOrders = await GetNumberOf(start, end);
                var result = totalRevenue / numberOfOrders;
                return result;
            }
        }

        public async Task<double> GetTotalFund()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<OrderDetail>().Include(d => d.Product).Where(d => d.Product != null).SumAsync(d => d.Product.BasePrice * d.Quantity);
                return result;
            }
        }

        public async Task<double> GetTotalFund(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                IEnumerable<Order> entities = await context.Set<Order>().Include(o => o.details).ThenInclude(d => d.Product).ThenInclude(d => d != null ? d.Type : null).Where(o => o.OrderDate >= start && o.OrderDate <= end).ToListAsync();

                double totalFund = 0;
                foreach (var entity in entities)
                {
                    foreach (var detail in entity.details)
                    {
                        if (detail.Product != null)
                        {
                            totalFund += detail.Product.BasePrice * detail.Quantity;
                        }
                    }
                }

                return totalFund;
            }
        }

        public async Task<double> GetAverageFund()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalFund = await GetTotalFund();
                var numberOfOrders = await GetNumberOf();
                var result = totalFund / numberOfOrders;
                return result;
            }
        }

        public async Task<double> GetAverageFund(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalFund = await GetTotalFund(start, end);
                var numberOfOrders = await GetNumberOf(start, end);
                var result = totalFund / numberOfOrders;
                return result;
            }
        }

        public async Task<double> GetTotalProfit()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalRevenue = await GetTotalRevenue();
                var totalFund = await GetTotalFund();
                var result = totalRevenue - totalFund;
                return result;
            }
        }

        public async Task<double> GetTotalProfit(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalRevenue = await GetTotalRevenue(start, end);
                var totalFund = await GetTotalFund(start, end);
                var result = totalRevenue - totalFund;
                return result;
            }
        }

        public async Task<double> GetAverageProfit()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalProfit = await GetTotalProfit();
                var numberOfOrders = await GetNumberOf();
                var result = totalProfit / numberOfOrders;
                return result;
            }
        }

        public async Task<double> GetAverageProfit(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var totalProfit = await GetTotalProfit(start, end);
                var numberOfOrders = await GetNumberOf(start, end);
                var result = totalProfit / numberOfOrders;
                return result;
            }
        }

        public async Task<int> GetNumberOf()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Order>().CountAsync();
                return result;
            }
        }

        public async Task<int> GetNumberOf(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var result = await context.Set<Order>().CountAsync(o => o.OrderDate >= start && o.OrderDate <= end);
                return result;
            }
        }

        public async Task<int> ProductSale(Product product)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var orderDetails = await context.Set<OrderDetail>()
                                                .Include(d => d.Order)
                                                .Where(d => d.Product.Id == product.Id)
                                                .ToListAsync();
                int quantitySold = orderDetails.Sum(d => d.Quantity);
                return quantitySold;
            }
        }

        public async Task<int> ProductSale(Product product, DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var orderDetails = await context.Set<OrderDetail>()
                                                .Include(d => d.Order)
                                                .Where(d => d.Product.Id == product.Id && d.Order.OrderDate >= start && d.Order.OrderDate <= end)
                                                .ToListAsync();
                int quantitySold = orderDetails.Sum(d => d.Quantity);
                return quantitySold;
            }
        }

        public async Task<List<Tuple<Product, int>>> SoldProducts(DateTime start, DateTime end)
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var allProducts = await context.Set<Product>().ToListAsync();

                var resultList = new List<Tuple<Product, int>>();

                foreach (var product in allProducts)
                {
                    var orderDetails = await context.Set<OrderDetail>()
                                                    .Include(d => d.Order)
                                                    .Where(d => d.Product.Id == product.Id && d.Order.OrderDate >= start && d.Order.OrderDate <= end)
                                                    .ToListAsync();
                    int quantitySold = orderDetails.Sum(d => d.Quantity);

                    resultList.Add(new Tuple<Product, int>(product, quantitySold));
                }

                return resultList;
            }
        }

        public async Task<List<Tuple<Product, int>>> SoldProducts()
        {
            using (GroceryStoreManagerDBContext context = new GroceryStoreManagerDBContext(_connectionString))
            {
                var allProducts = await context.Set<Product>().ToListAsync();

                var resultList = new List<Tuple<Product, int>>();

                foreach (var product in allProducts)
                {
                    var orderDetails = await context.Set<OrderDetail>()
                                                    .Include(d => d.Order)
                                                    .Where(d => d.Product.Id == product.Id)
                                                    .ToListAsync();
                    int quantitySold = orderDetails.Sum(d => d.Quantity);

                    resultList.Add(new Tuple<Product, int>(product, quantitySold));
                }

                return resultList;
            }
        }

        public async Task<List<Tuple<Product, int>>> HotProducts(int top)
        {
            var soldProducts = await SoldProducts();

            var hotProducts = soldProducts.OrderByDescending(p => p.Item2).Take(top).ToList();

            return hotProducts;
        }

        public async Task<List<Tuple<Product, int>>> HotProducts(DateTime start, DateTime end, int top)
        {
            var soldProducts = await SoldProducts(start, end);

            var hotProducts = soldProducts.OrderByDescending(p => p.Item2).Take(top).ToList();

            return hotProducts;
        }
    }
}
