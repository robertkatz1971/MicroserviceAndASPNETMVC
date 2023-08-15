using System;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
	public class Coupon
	{
		[Key]
		public int CouponId { get; set; }
		[Required]
		public string CouponCode { get; set; } = String.Empty;
		[Required]
		public double DiscountAmount { get; set; }
		[Range(0, 100)]
		public int MinAmount { get; set; }		
	}
}

