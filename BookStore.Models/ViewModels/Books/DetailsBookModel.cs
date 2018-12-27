using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Books
{
    public class DetailsBookModel
    {
        public string Title { get; set; }

        public string[] Authors { get; set; }

        public string[] Categories { get; set; }

        public string Img { get; set; }

        public string Description { get; set; }

        public string Isbn { get; set; }

        public decimal Price { get; set; }

        public string ReleaseDate { get; set; }
    }
}
