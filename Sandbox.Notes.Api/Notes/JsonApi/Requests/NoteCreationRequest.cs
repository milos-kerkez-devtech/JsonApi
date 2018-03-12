using System.Runtime.Serialization;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.JsonApi.Attributes;
using Sandbox.Notes.Api.Notes.JsonApi.Resources;

namespace Sandbox.Notes.Api.Notes.JsonApi.Requests
{
    [DataContract]
    public class NoteCreationRequest: JsonApiDocument<NoteResource<NewNoteAttributes>>
    {
    }
}
