using Microsoft.AspNetCore.Antiforgery;

namespace ProcurementMaterialAPI
{
	public class XsrfProtectionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IAntiforgery _antiforgery;

		public XsrfProtectionMiddleware(RequestDelegate next, IAntiforgery antiforgery)
		{
			_next = next;
			_antiforgery = antiforgery;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == "/user/auth") 
			{
				await _next(context);
				return;
			}

			var tokens = _antiforgery.GetAndStoreTokens(context);
			context.Response.Cookies.Append(".AspNetCore.Xsrf", tokens.RequestToken,
				new CookieOptions { HttpOnly = false, Secure = true, MaxAge = TimeSpan.FromMinutes(60) });

			if (HttpMethods.IsPost(context.Request.Method))
			{
				await _antiforgery.ValidateRequestAsync(context);
			}

			await _next(context);
		}
	}
}
