using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model;

public class OrderDetail
{
    [Key]
    public Order? order;
    [Key]
    public Product? product;

    public int Quantity;
}
