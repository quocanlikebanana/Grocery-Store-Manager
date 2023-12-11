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
        [Key]
        public int? OrderId { get; set; }
        [Key]
        public int? ProductId { get; set; } 
        
        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }

        public int? Quantity { get; set; }
    }
}
