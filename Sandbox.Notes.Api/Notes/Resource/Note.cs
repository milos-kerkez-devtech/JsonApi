using System;
using System.ComponentModel.DataAnnotations;

namespace Sandbox.Notes.Api.Notes.Resource
{
	public class Note
	{
	    public int Id { get; set; }
	    [Required(ErrorMessage = "Note must note be empty")]
	    public string Text { get; set; }
	    public string Creator { get; set; }
	    public DateTime CreatedOn { get; set; }

	    public object Clone()
	    {
	        return (Note)MemberwiseClone();
	    }
    }
}