using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sandbox.Notes.Api.Infrastructure.DataAccess;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.Services
{

    public interface IReadNoteListService
    {
        IEnumerable<NoteList> Get();
        NoteList Get(int id);
    }
    public class ReadNoteListService : IReadNoteListService
    {

        private readonly NotesDbContext _context;

        public ReadNoteListService(NotesDbContext context)
        {
            _context = context;
        }
        public IEnumerable<NoteList> Get()
        {
            var noteLists = _context.NoteLists.AsEnumerable();
            return noteLists;
        }

        public NoteList Get(int id)
        {
            var entity = _context.NoteLists.FirstOrDefault(n=>n.Id == id);
            return (NoteList) entity?.Clone();
        }
    }
}
