using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sandbox.Notes.Api.Infrastructure.Logging;


namespace Sandbox.Notes.Api.Infrastructure.Validation
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class ValidateModelStateAttribute : ActionFilterAttribute
	{
		public ValidateModelStateAttribute()
		{
			
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new BadRequestObjectResult(new ErrorResponse { Errors = context.ModelState.GetErrors() });
			}
		}
	}
}