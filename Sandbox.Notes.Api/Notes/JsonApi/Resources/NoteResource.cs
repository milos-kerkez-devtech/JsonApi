using System.Runtime.Serialization;
using AppRiver.JsonApi;
using Sandbox.Notes.Api.Notes.JsonApi.Attributes;
using Sandbox.Notes.Api.Notes.JsonApi.Relationships;

namespace Sandbox.Notes.Api.Notes.JsonApi.Resources
{
    [DataContract]
    public class NoteResource : NoteResource<NoteAttributes>
    {
    }

    [DataContract]
    [JsonApiResource("notes")]
    public class NoteResource<TAttributes> : JsonApiResource<TAttributes> where TAttributes : new()
    {
        [DataMember(Name = "relationships", EmitDefaultValue = false)]
        public NoteRelationships Relationships { get; set; }
    }
}
