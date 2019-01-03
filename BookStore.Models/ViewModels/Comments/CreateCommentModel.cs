using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Models.ViewModels.Comments
{
    public class CreateCommentModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(40)]
        public string Title { get; set; }

        public int BookId { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(350)]
        public string Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
