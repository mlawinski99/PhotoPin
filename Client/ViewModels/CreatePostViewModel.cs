using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    public class CreatePostViewModel
    {
        [Required]
        public IFormFile Image { get; set; }
		[Required]
		public string Description { get; set; }
    }
}
