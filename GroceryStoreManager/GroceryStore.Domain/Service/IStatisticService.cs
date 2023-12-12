using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Service
{
    public  interface IStatisticService
    {
        Task<double> GetTotalRevenue();

        Task<double> GetTotalRevenue(DateTime start, DateTime end);
        Task<double> GetAverageRevenue();
        Task<double> GetAverageRevenue(DateTime start, DateTime end);
        Task<int> GetNumberOf();

    }
}
