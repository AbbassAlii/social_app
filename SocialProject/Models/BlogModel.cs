using System.ComponentModel.DataAnnotations;

namespace SocialProject.Models
{
	public class BlogModel
	{
		[Key]
		public int BlogId { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public string? File { get; set; }
		public string? CreateBy { get; set; }
		public string? CreateDate { get; set; }
		public string? UpdateBy { get; set; }
		public string? UpdateDate { get; set; }
		public string? Status { get; set; }
		public string UserID { get; set; }
	}
}
