using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ProductType? Type { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Image {  get; set; }
    }
}
