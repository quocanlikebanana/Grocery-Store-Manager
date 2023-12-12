﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{
    public class Product
    {
        public Product()
        {
        }

        public Product(int? id, string name, int typeId, ProductType type, double price, int quantity, double basePrice)
        {
            Id = id;
            Name = name;
            TypeId = typeId;
            Type = type;
            Price = price;
            Quantity = quantity;
            BasePrice = basePrice;
        }

        [Key]
        public int? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public ProductType? Type { get; set; } = null;
        public double Price { get; set; }
        public int Quantity { get; set; }

        public double BasePrice { get; set; }
    }
}
