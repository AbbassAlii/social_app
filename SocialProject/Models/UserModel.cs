using System.ComponentModel.DataAnnotations;

namespace SocialProject.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
     
        public string? Gender { get; set; }
        public string? UserRole { get; set; }
        public string? Address { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdateDate { get; set; }
        public string? Status { get; set; }
 

		// Navigation property for one-to-many relationship
		//public ICollection<PostModel> Posts { get; set; }
        //public PostModel posts { get; set; } //[ many user 1 post]
        //public EventModel eventModel { get; set; } //[ many user 1 event]

        //public BlogModel blogModel { get; set; } //[ many user 1 event]
        //public UserCommentModel UserCommentModel { get; set; }
    }


}

