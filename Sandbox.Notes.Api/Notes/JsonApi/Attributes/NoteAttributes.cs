using System;
using System.Runtime.Serialization;

namespace Sandbox.Notes.Api.Notes.JsonApi.Attributes
{
    [DataContract]
    public class NoteAttributes
    {
        [DataMember(Name = "text", EmitDefaultValue = false)]
        public string Text { get; set; }

        [DataMember(Name = "creator", EmitDefaultValue = false)]
        public string Creator { get; set; }

        [DataMember(Name = "createdOn", EmitDefaultValue = false)]
        public DateTime CreatedOn { get; set; }
    }
}
