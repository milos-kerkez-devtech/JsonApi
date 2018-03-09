namespace Sandbox.Notes.Api.Infrastructure.Utility
{
	public static class ResponseFactory
	{
		public static Response<TData> Success<TData>(TData data) where TData : new()
		{
			return new Response<TData>(data);
		}

		public static Response<TData> Fail<TData>(string message) where TData : new()
		{
			return new Response<TData>(message);
		}
	}
}