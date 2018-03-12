using System.Runtime.Serialization;
using AppRiver.JsonApi;

namespace Sandbox.Notes.Api.Notes.JsonApi
{
    [DataContract]
    public class NoteCreationRequest: JsonApiDocument<NoteResource<NewNoteAttributes>>
    {
    }
}
