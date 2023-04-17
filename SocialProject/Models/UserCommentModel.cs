using System.ComponentModel.DataAnnotations;

namespace SocialProject.Models
{
	public class UserCommentModel
	{
		[Key]
		public int UserCommentId { get; set; }
		public string? CommentText { get; set; }
		public string? Reaction { get; set; }
		public string? CreateBy { get; set; }
		public string? CreateDate { get; set; }
		public string? UpdateBy { get; set; }
		public string? UpdateDate { get; set; }
        //public string UserID { get; set; }
        //public string PostID { get; set; }
        //public string BlogID { get; set; }
       // public virtual ICollection<UserModel> UserModel { get; set; }
       //// public virtual ICollection<PostModel> PostModel { get; set; }
       // public virtual ICollection<EventModel> EventModel { get; set; }
       // public virtual ICollection<BlogModel> BlogModel { get; set; }
    }
}