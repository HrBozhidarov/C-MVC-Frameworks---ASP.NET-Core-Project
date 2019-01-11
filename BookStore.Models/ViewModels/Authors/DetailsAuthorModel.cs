using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Authors
{
    public class DetailsAuthorModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public string Details { get; set; }
    }
}
