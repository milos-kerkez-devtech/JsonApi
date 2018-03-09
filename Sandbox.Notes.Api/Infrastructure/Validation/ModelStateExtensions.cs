using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sandbox.Notes.Api.Infrastructure.Validation
{
	public static class ModelStateExtensions
	{
		public static IList<string> GetErrors(this ModelStateDictionary modelState)
		{
			var validationErrors = new List<string>();

			foreach (var state in modelState)
			{
				validationErrors.AddRange(state.Value.Errors
					.Select(error => error.ErrorMessage)
					.ToList());
			}

			return validationErrors;
		}
	}
}