using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Sandbox.Notes.Api.Infrastructure.DataAccess;
using Sandbox.Notes.Api.Infrastructure.Utility;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.Services
{
	public interface IDeleteNoteService
	{
		Task<Response<bool>> Delete(int noteId);
	}

	public class DeleteNoteService : IDeleteNoteService
	{
		private readonly NotesDbContext _context;
		private readonly ILogger<DeleteNoteService> _logger;

		public DeleteNoteService(NotesDbContext context, ILogger<DeleteNoteService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<Response<bool>> Delete(int noteId)
		{
			try
			{
				var noteToRemove = await _context.Notes.FindAsync(noteId);
				_context.Notes.Remove(noteToRemove);
				await _context.SaveChangesAsync();
				return ResponseFactory.Success(true);
			}
			catch (Exception exception)
			{
				_logger.LogError($"Failed to delete note with id: {noteId}", exception);
				return ResponseFactory.Fail<bool>($"Failed to delete note with id: {noteId}");
			}
		}
	}
}