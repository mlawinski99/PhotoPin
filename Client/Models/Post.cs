﻿using Newtonsoft.Json.Linq;

namespace Client.Models
{
	public class Post
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		//public FormFile Image { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
		public int? likeCount { get; set; }
		public List<Comment> Comments { get; set; }
	}
}
