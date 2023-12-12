using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(int id, string name, double moneyForPromotion, int couponCount, string tel, string address)
        {
            Id = id;
            Name = name;
            MoneyForPromotion = moneyForPromotion;
            CouponCount = couponCount;
            Tel = tel;
            Address = address;
        }

        [Key]
        public int? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;

        public double MoneyForPromotion { get; set; }

        public int CouponCount { get; set; }

        public string Tel { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}  
