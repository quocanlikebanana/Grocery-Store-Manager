﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Domain.Model;

namespace GroceryStore.MainApp.Models.PreModel;
public class PMOrder
{
    public PMOrder(Order? order = null)
    {
        // Edit case
        if (order != null)
        {
            Id = order.Id;
            Customer = order.Customer;
            Details = order.details ?? new();
            OrderDate = order.OrderDate;
        }
    }

    public int? Id { get; set; } = null;
    public Customer? Customer { get; set; } = null;
    public List<OrderDetail> Details { get; set; } = new();
    public int CouponUsed { get; set; } = 0;
    public double DiscountPerCoupon { get; set; } = 0;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public double DiscountMoney => CouponUsed * DiscountPerCoupon;
    public double TotalPrice
    {
        get
        {
            var totalPrice = 0.0;
            foreach (var item in Details)
            {
                totalPrice += (item.Product?.Price ?? 0) * item.Quantity;
            }
            totalPrice -= DiscountMoney;
            return totalPrice;
        }
    }

    //TotalPrice = Math.Min( SelectedProduct?.Price ?? 0 * Quantity - SelectedCustomer?.Coupons.Count ?? 0 * SelectedCustomer?.MoneyForPromotion ?? 0, 0),
    public Order Get()
    {
        var order = new Order()
        {
            Id = Id ?? -1,
            CustomerID = Customer?.Id ?? -1,
            Customer = Customer,
            details = Details,
            OrderDate = OrderDate,
            TotalPrice = TotalPrice,
            TotalDiscount = DiscountMoney,
        };
        return order;
    }


    //public void Input(Customer customer, List<OrderDetail> details, DateTime orderDate, double totalPrice, double totalDiscount)
    //{

    //}

    //public static Order? New(Customer customer, List<OrderDetail> details, DateTime orderDate, double totalPrice, double totalDiscount)
    //{
    //    var order = new Order()
    //    {
    //        CustomerID = customer.Id,
    //        details = details,
    //        OrderDate = orderDate,
    //        TotalPrice = totalPrice,
    //        TotalDiscount = totalDiscount,
    //    };
    //    return order;
    //}
}