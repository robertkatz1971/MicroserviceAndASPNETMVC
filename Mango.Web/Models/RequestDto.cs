using System;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Models
{
	public class RequestDto
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
		public string Url { get; set; } = String.Empty;
		public object? Data { get; set; }
		public string AccessToken { get; set; } = String.Empty;
	}
}

