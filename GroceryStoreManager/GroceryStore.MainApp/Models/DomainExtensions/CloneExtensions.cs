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
            order = new Order()
            {
                Id = 0,
                Customer = source.order?.Customer ?? null,
                OrderDate = source.order?.OrderDate ?? new DateTime(),
                TotalPrice = source.order?.TotalPrice ?? 0,
            },
            product = source.product,
            Quantity = source.Quantity,
        };
        return orderDetail;
    }
}
