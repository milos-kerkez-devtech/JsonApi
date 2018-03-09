using Microsoft.EntityFrameworkCore;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Infrastructure.DataAccess
{
	public class NotesDbContext : DbContext
	{
		public NotesDbContext(DbContextOptions<NotesDbContext> contextOptions) 
			: base(contextOptions)
		{
			
		}

		public DbSet<Note> Notes { get; set; }
	}
}