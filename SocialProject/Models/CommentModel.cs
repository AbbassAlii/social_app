using System.ComponentModel.DataAnnotations;

namespace SocialProject.Models
{
	public class CommentModel
	{
		[Key]
		public int CommentId { get; set; }	
		public string? CommentText { get; set; }	
		public string? Reaction { get; set; }
		//public string UserID { get; set; }
		//public string PostID { get; set; }
		//public string BlogID { get; set; }
	}
}
