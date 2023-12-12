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
        public ProductType()
        {
        }

        public ProductType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [Key]
        public int? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
    }
}
