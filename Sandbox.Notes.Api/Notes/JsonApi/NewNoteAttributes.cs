using System.Runtime.Serialization;

namespace Sandbox.Notes.Api.Notes.JsonApi
{
    [DataContract]
    public class NewNoteAttributes
    {
        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }

        [DataMember(Name = "creator", EmitDefaultValue = false)]
        public string Creator { get; set; }
    }
}
