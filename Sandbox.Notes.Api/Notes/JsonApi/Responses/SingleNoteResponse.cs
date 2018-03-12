using System.Runtime.Serialization;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.JsonApi.Resources;

namespace Sandbox.Notes.Api.Notes.JsonApi.Responses
{

    [DataContract]
    public class SingleNoteResponse : JsonApiDocument<NoteResource>
    {
    }
}
