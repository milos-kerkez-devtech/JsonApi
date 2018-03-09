using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sandbox.Notes.Api.Infrastructure.DataAccess;
using Sandbox.Notes.Api.Infrastructure.Utility;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.Services
{
    public interface IReadNoteService
    {
        IEnumerable<Note> Get();
        Task<Response<Note>> Get(int noteId);
    }

    public class ReadNoteService : IReadNoteService
    {
        private readonly NotesDbContext _context;
        private readonly ILogger<ReadNoteService> _logger;

        public ReadNoteService(NotesDbContext context, ILogger<ReadNoteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Note> Get()
        {

            var notes = _context.Notes.AsEnumerable();
            return notes;
        }

        public async Task<Response<Note>> Get(int noteId)
        {
            try
            {
                var note = await _context.Notes.FindAsync(noteId);
                return ResponseFactory.Success(note);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get note by id: {noteId}", exception);
                return ResponseFactory.Fail<Note>($"Failed to get note by id: {noteId}");
            }
        }
    }
}