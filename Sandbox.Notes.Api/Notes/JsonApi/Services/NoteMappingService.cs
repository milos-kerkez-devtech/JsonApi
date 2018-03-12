using System;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.JsonApi.Attributes;
using Sandbox.Notes.Api.Notes.JsonApi.Relationships;
using Sandbox.Notes.Api.Notes.JsonApi.Resources;
using Sandbox.Notes.Api.Notes.Resource;

namespace Sandbox.Notes.Api.Notes.JsonApi.Services
{
    public class NoteMappingService
    {
        public NoteResource MapEntityToResource(Note entity)
        {
            if (entity == null) return null;

            return new NoteResource
            {
                Id = entity.Id.ToString(),
                Attributes = new NoteAttributes
                {
                    CreatedOn = entity.CreatedOn,
                    Creator = entity.Creator,
                    Text = entity.Text
                },
                Relationships = new NoteRelationships
                {
                    NoteList = new JsonApiToOneRelationship
                    {
                        Data = new JsonApiResourceIdentifier("noteLists", entity.NoteListId.ToString())
                    }
                }
            };
        }

        public Note MapResourceToEntity(NoteResource<NewNoteAttributes> resource)
        {
            if (resource == null) return null;
            var attributes = resource.Attributes ?? new NewNoteAttributes();
            return new Note()
            {
                Text = attributes.Text,
                Creator= attributes.Creator,
                CreatedOn = DateTime.Now
            };
        }
    }
}
