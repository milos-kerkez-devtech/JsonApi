using System;

namespace Sandbox.Notes.Api.Notes.Resource
{
    public class NoteList: ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return (NoteList)MemberwiseClone();
        }
    }
}
