using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Books
{
    public class VisualizeBooktemsModel
    {
        public int Id { get; set; }

        //public int Number { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        //public decimal TotalPrice { get; set; }
    }
}
