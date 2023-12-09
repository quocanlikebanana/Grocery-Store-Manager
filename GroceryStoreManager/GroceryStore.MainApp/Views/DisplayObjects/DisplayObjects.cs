using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Model;
using GroceryStore.MainApp.Models.DomainExtensions;

namespace GroceryStore.MainApp.Views.DisplayObjects;

// Read only classes


public class OrderDisplay
{
    private readonly OrderDetail _orderDetail;
    public OrderDisplay(OrderDetail orderDetail)
    {
        _orderDetail = orderDetail;
    }
    public DateTime? OrderDate => _orderDetail.Order?.OrderDate ?? null;
    public string? ProductName => _orderDetail.Product?.Name ?? null;
    public int? Quantity => _orderDetail.Quantity;
    public string? CustomerName => _orderDetail.Order?.Customer?.Name ?? null;
    public double? TotalPrice => _orderDetail.Order?.TotalPrice ?? null;
}