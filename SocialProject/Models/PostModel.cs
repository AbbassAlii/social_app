using System.ComponentModel.DataAnnotations;

namespace SocialProject.Models
{
	public class PostModel
	{
		[Key]
		public int PostId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public string? Activity { get; set; }
		public string? Attachment { get; set; }
		public string? CreateBy { get; set; }
		public string? CreateDate { get; set; }
		public string? UpdateBy { get; set; }
		public string? UpdateDate { get; set; }
		public string? Status { get; set; }

		// Foreign key property
		public int? UserId { get; set; } 
        //public int UserCommentId { get; set; }
        // Navigation property for the related user [user many post one]
        // public UserModel User { get; set; }
        // ICollection<UserModel> User { get; set; }//[user many blog one]
       // public ICollection<UserCommentModel> UserComment { get; set; }

    }
}
