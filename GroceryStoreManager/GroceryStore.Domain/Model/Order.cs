using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{
    public class Order
    {
        public Order()
        {
        }

        public Order(int id, int customerID, Customer customer, List<OrderDetail> details, DateTime orderDate, double totalPrice, double totalDiscount)
        {
            Id = id;
            CustomerID = customerID;
            Customer = customer;
            this.details = details;
            OrderDate = orderDate;
            TotalPrice = totalPrice;
            TotalDiscount = totalDiscount;
        }

        [Key]
        public int? Id { get; set; } = null;

        public int? CustomerID { get; set; } = null;
        public virtual Customer? Customer { get; set; } = null;
        public virtual List<OrderDetail> details { get; set; } = new List<OrderDetail>();
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public double TotalDiscount { get; set; }
    }
}
