using System.ComponentModel.DataAnnotations;


namespace SocialProject.Models
{
	public class PollModel
	{
		[Key]
		public int PollId { get; set; }	
		public string? PollTitle { get; set; }
		public string? PollChoices { get; set;}
		//public string UserID { get; set; }

	}
}
