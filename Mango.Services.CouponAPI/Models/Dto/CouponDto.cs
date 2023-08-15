﻿using System;
namespace Mango.Services.CouponAPI.Models.Dto
{
	public class CouponDto
	{
        public int CouponId { get; set; }
        public string CouponCode { get; set; } = String.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}

