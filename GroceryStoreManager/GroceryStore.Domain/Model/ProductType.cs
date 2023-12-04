using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{
    public class ProductType
    {
        [Key]
        public string? Name { get; set; }
        public  string? Image {  get; set; }
    }
}
