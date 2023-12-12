﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Domain.Model
{ 
    public class Coupon
    {
        public Coupon()
        {
        }

        public Coupon(int id, double thresHold, double perCoupon)
        {
            Id = id;
            ThresHold = thresHold;
            this.perCoupon = perCoupon;
        }

        [Key]
        public int? Id { get; set; } = null;
        public double ThresHold {  get; set; }
        public double perCoupon { get; set; }
    }
}
