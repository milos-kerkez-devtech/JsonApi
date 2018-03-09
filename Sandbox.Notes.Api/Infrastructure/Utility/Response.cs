using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Notes.Api.Infrastructure.Utility
{
	public class Response<TData> where TData : new()
	{
		public Response(TData data)
		{
			IsSuccess = true;
			Data = data;
			Messages = new List<string>();
		}

		public Response(string message)
		{
			IsSuccess = false;
			Messages = new List<string> {message};
		}

		public TData Data { get;}
		public bool IsSuccess { get; }
		public IList<string> Messages { get; }

	}
}