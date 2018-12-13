using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Books
{
    public class BookDisplayModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Authors { get; set; }

        public decimal Price { get; set; }

        public string ImgUrl { get; set; }
    }
}
