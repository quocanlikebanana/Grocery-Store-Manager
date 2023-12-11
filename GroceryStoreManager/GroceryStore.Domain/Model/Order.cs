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
        [Key]
        public int Id { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual List<OrderDetail> details { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
    }
}
