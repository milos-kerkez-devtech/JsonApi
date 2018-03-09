using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sandbox.Notes.Api.Infrastructure.Validation;

namespace Sandbox.Notes.Api.Infrastructure.Logging
{
	public class HttpGlobalExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<HttpGlobalExceptionFilter> _logger;

		public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
		{
			_logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			var exception = context.Exception as ValidationException;
			if (exception != null)
			{
				var validationContext = exception;
				context.Result = new BadRequestObjectResult(new ErrorResponse {Errors = validationContext.Errors});
			}
			else
			{
				context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
			}

			_logger.LogError(context.Exception.Message, context.Exception);
			context.ExceptionHandled = true;
		}
	}

	public class ErrorResponse
	{
		public IList<string> Errors { get; set; }
	}
}