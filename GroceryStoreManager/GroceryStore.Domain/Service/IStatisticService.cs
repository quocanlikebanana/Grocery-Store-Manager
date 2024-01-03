using GroceryStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Service
{
    public interface IStatisticService
    {
        Task<double> GetTotalRevenue();
        Task<double> GetTotalRevenue(DateTime start, DateTime end);
        Task<double> GetAverageRevenue();
        Task<double> GetAverageRevenue(DateTime start, DateTime end);
        Task<double> GetTotalFund();
        Task<double> GetTotalFund(DateTime start, DateTime end);
        Task<double> GetAverageFund();
        Task<double> GetAverageFund(DateTime start, DateTime end);
        Task<double> GetTotalProfit();
        Task<double> GetTotalProfit(DateTime start, DateTime end);
        Task<double> GetAverageProfit();
        Task<double> GetAverageProfit(DateTime start, DateTime end);
        Task<int> GetNumberOf();
        Task<int> GetNumberOf(DateTime start, DateTime end);
        Task<int> ProductSale(Product product);
        Task<int> ProductSale(Product product, DateTime start, DateTime end);
        Task<List<Tuple<Product, int>>> SoldProducts();
        Task<List<Tuple<Product, int>>> SoldProducts(DateTime start, DateTime end);
        Task<List<Tuple<Product, int>>> HotProducts(int top);
        Task<List<Tuple<Product, int>>> HotProducts(DateTime start, DateTime end, int top);
    }
}
