using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Model;

namespace GroceryStore.MainApp.Models.DomainExtensions;

public static class CloneExtensions
{
}


public static class OrderDetailCloneExtensions
{
    public static OrderDetail Clone(this OrderDetail source)
    {
        var orderDetail = new OrderDetail()
        {
            Order = new Order()
            {
                Id = 0,
                Customer = source.Order?.Customer ?? null,
                OrderDate = source.Order?.OrderDate ?? new DateTime(),
                TotalPrice = source.Order?.TotalPrice ?? 0,
            },
            Product = source.Product,
            Quantity = source.Quantity,
        };
        return orderDetail;
    }
}
