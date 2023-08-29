using System;
namespace Mango.Web.Utility
{
	public class StaticDetails
	{
		public static string CouponAPIBase { get; set; } = String.Empty;
        public static string AuthAPIBase { get; set; } = String.Empty;
        public static string ProductAPIBase { get; set; } = String.Empty;
        public const string RoleAdmin = "ADMIN";
		public const string RoleCustomer = "CUSTOMER";
		public const string TokenCookie = "JWTToken";

        public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}

