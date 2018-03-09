using System.Collections.Generic;
using System.Runtime.Serialization;
using AppRiver.JsonApi;

namespace Sandbox.Notes.Api.Notes.JsonApi
{
    [DataContract]
    public class ListOfNotesResponse: JsonApiDocument<IList<NoteResource>>
    {
    }
}
