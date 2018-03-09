using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sandbox.Notes.Api.Infrastructure.DataAccess;
using Sandbox.Notes.Api.Infrastructure.Utility;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.Services
{
	public interface IUpdateNoteService
	{
		Task<Response<Note>> Update(int noteId, Note note);
	}

	public class UpdateNoteService : IUpdateNoteService
	{
		private readonly NotesDbContext _context;
		private readonly ILogger<UpdateNoteService> _logger;

		public UpdateNoteService(NotesDbContext context, ILogger<UpdateNoteService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<Response<Note>> Update(int noteId, Note note)
		{
			try
			{
				var noteToUpdate = await _context.Notes.FindAsync(noteId);
				noteToUpdate.Text = note.Text;
				_context.Notes.Update(noteToUpdate);
				await _context.SaveChangesAsync();
				return ResponseFactory.Success(noteToUpdate);
			}
			catch (Exception exception)
			{
				_logger.LogError($"Failed to update note with id: {noteId}", exception);
				return ResponseFactory.Fail<Note>($"Failed to update note with id: {noteId}");
			}
		}
	}
}