using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Model;

namespace GroceryStore.MainApp.Models.Extensions;

public static class ModelExtensions
{
}

public static class CustomerExtensions
{
    public static Customer Clone(this Customer source)
    {
        return new Customer()
        {
            Id = source.Id,
            Address = source.Address,
            CouponCount = source.CouponCount,
            MoneyForPromotion = source.MoneyForPromotion,
            Name = source.Name,
            Tel = source.Tel,
        };
    }
}
