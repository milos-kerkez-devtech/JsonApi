using System.Collections.Generic;
using System.Runtime.Serialization;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.JsonApi.Resources;

namespace Sandbox.Notes.Api.Notes.JsonApi.Responses
{
    [DataContract]
    public class ListOfNotesResponse: JsonApiDocument<IList<NoteResource>>
    {
    }
}
