using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.JsonApi
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
