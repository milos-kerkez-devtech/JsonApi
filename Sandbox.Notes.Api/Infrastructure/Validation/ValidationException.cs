using System;
using System.Collections.Generic;

namespace Sandbox.Notes.Api.Infrastructure.Validation
{
	public class ValidationException : Exception
	{
		public IList<string> Errors { get; }

		public ValidationException(IList<string> errors)
		{
			Errors = errors;
		}
	}
}