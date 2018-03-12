using System.Runtime.Serialization;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.JsonApi.Attributes;

namespace Sandbox.Notes.Api.Notes.JsonApi.Resources
{
    [DataContract]
    public class NoteListResource: NoteListResource<NoteListAttributes>
    {
    }

    [DataContract]
    [JsonApiResource("noteLists")]
    public class NoteListResource<TAttributes> : JsonApiResource<TAttributes> where TAttributes : new()
    {
        
    }
}


