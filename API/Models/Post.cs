﻿using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; }

    }
}
