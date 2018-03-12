using Sandbox.Notes.Api.Notes.JsonApi.Attributes;
using Sandbox.Notes.Api.Notes.JsonApi.Resources;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.JsonApi.Services
{
    public class NoteListMappingService
    {
        public NoteListResource MapEntityToResource(NoteList entity)
        {
            if (entity == null) return null;

            return new NoteListResource
            {
                Id = entity.Id.ToString(),
                Attributes = new NoteListAttributes()
                {
                    Id = entity.Id,
                    Name = entity.Name
                }
            };
        }
    }
}
