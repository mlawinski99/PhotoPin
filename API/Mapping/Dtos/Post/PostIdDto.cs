using System.ComponentModel.DataAnnotations;

namespace API.Mapping.Dtos.Post
{
	public class PostIdDto
	{
		[Required]
		public int id { get; set; }
		[Required]
		public string userId { get; set; }
	}
}
