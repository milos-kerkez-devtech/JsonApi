using System.Runtime.Serialization;

namespace Sandbox.Notes.Api.Notes.JsonApi.Attributes
{

    [DataContract]
    public class NoteListAttributes
    {
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int Id { get; set; }
    }
}