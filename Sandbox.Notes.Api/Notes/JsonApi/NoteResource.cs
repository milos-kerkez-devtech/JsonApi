using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AppRiver.JsonApi;

namespace Sandbox.Notes.Api.Notes.JsonApi
{
[DataContract]
    public class NoteResource: NoteResource<NoteAttributes>
    {
    }

    [DataContract]
    [JsonApiResource("notes")]
    public class NoteResource<TAttributes> : JsonApiResource<TAttributes> where TAttributes : new()
    {
        
    }
}
