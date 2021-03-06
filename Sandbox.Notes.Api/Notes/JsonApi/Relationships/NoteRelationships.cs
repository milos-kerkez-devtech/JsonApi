﻿using System.Runtime.Serialization;
using AppRiver.JsonApi;

namespace Sandbox.Notes.Api.Notes.JsonApi.Relationships
{
    [DataContract]
    public class NoteRelationships
    {
    [DataMember(Name="noteList", EmitDefaultValue = false)]
    public JsonApiToOneRelationship NoteList { get; set; }
    }
}
