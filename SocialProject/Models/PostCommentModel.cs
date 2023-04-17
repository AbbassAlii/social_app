using System.ComponentModel.DataAnnotations;

namespace SocialProject.Models
{
	public class PostCommentModel
	{
		[Key]
		public int PostCommentId { get; set; }	
		public string? CommentText { get; set; }	
		public string? Reaction { get; set; }
		public string? CreateBy { get; set; }
		public string? CreateDate { get; set; }
		public string? UpdateBy { get; set; }
		public string? UpdateDate { get; set; }
		//public string UserID { get; set; }
		//public string PostID { get; set; }
		//public string BlogID { get; set; }
	}
}
