using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sandbox.Notes.Api.Infrastructure.Logging
{
	public class CorrelationMiddleware
	{
		private readonly RequestDelegate _next;

		public CorrelationMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public Task Invoke(HttpContext context)
		{
			var correlationIdString = context.Request.Headers["X-Correlation-Id"];
			if (Guid.TryParse(correlationIdString, out Guid correlationId))
			{
				context.TraceIdentifier = correlationIdString;
			}

			return this._next(context);
		}
	}
}