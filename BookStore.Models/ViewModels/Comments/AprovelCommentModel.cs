using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Comments
{
    public class AprovelCommentModel
    {
        public string ImgUrl { get; set; }

        public int Id { get; set; }

        public string TitleBook { get; set; }

        public string Username { get; set; }

        public string Content { get; set; }

        public string PostedOn { get; set; }

        public string Title { get; set; }

        public double Rating { get; set; }

        public int BookId { get; set; }
    }
}
