using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.JsonApi
{
    public class NoteMappingService
    {
        public NoteResource MapEntityToResource(Note entity)
        {
            if (entity == null) return null;

            return new NoteResource()
            {
                Id = entity.Id.ToString(),
                Attributes = new NoteAttributes
                {
                    CreatedOn = entity.CreatedOn,
                    Creator = entity.Creator,
                    Text = entity.Text
                }
            };
        }

        //public IterationEntity MapResourceToEntity(IterationResource<NewIterationAttributes> resource)
        //{
        //    if (resource == null) return null;
        //    var attributes = resource.Attributes ?? new NewIterationAttributes();
        //    return new IterationEntity
        //    {
        //        Id = Guid.Parse(resource.Id),
        //        Name = attributes.Name,
        //        StartDate = attributes.StartDate,
        //        EndDate = attributes.EndDate
        //    };
        //}

        //public void ApplyUpdates(IterationEntity entity, IterationResource<UpdatableIterationAttributes> resourceUpdates)
        //{
        //    if (entity == null) return;
        //    if (resourceUpdates == null) return;

        //    var attributes = resourceUpdates.Attributes;
        //    if (attributes != null)
        //    {
        //        if (attributes.Name != null) entity.Name = attributes.Name;
        //        if (attributes.StartDate != null) entity.StartDate = attributes.StartDate.Value;
        //        if (attributes.EndDate != null) entity.EndDate = attributes.EndDate.Value;
        //    }
        //}
    }
}
