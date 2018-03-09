using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sandbox.Notes.Api.Infrastructure.DataAccess;
using Sandbox.Notes.Api.Infrastructure.Utility;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.Services
{
	public interface ICreateNoteService
	{
		Task<Response<Note>> Create(Note note);
	}

	public class CreateNoteService : ICreateNoteService
	{
		private readonly NotesDbContext _context;
		private readonly ILogger<CreateNoteService> _logger;

		public CreateNoteService(NotesDbContext context, ILogger<CreateNoteService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task<Response<Note>> Create(Note note)
		{
			try
			{
				var createdNote = await _context.Notes.AddAsync(note);
				await _context.SaveChangesAsync();
				return ResponseFactory.Success(createdNote.Entity);
			}
			catch (Exception exception)
			{
				_logger.LogError("Failed to create note", exception);
				return ResponseFactory.Fail<Note>("Failed to create note");
			}
		}
	}
}