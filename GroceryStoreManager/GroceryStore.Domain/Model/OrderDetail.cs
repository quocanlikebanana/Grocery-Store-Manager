using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{
    public class OrderDetail
    {
        public OrderDetail()
        {
        }

        public OrderDetail(int orderId, int productId, Order order, Product product, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            Order = order;
            Product = product;
            Quantity = quantity;
        }

        [Key]
        public int? OrderId { get; set; } = null;
        [Key]
        public int ProductId { get; set; } 
        
        public virtual Order? Order { get; set; } = null;

        public virtual Product? Product { get; set; } = null;

        public int Quantity { get; set; }
    }
}
