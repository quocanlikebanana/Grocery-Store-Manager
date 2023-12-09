using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Models.DomainExtensions;

namespace GroceryStore.MainApp.Views;

// Read only classes


public class OrderDisplay
{
    private readonly OrderDetail _orderDetail;
    public OrderDisplay(OrderDetail orderDetail)
    {
        _orderDetail = orderDetail;
    }
    public DateTime? OrderDate => _orderDetail.order?.OrderDate ?? null;
    public string? ProductName => _orderDetail.product?.Name ?? null;
    public int? Quantity => _orderDetail.Quantity;
    public string? CustomerName => _orderDetail.order?.Customer?.Name ?? null;
    public double? TotalPrice => _orderDetail.order?.TotalPrice ?? null;
}