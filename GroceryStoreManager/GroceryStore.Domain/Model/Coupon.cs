using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{ 
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        public double ThresHold {  get; set; }
        public double perCoupon { get; set; }
    }
}
