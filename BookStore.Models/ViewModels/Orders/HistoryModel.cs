using BookStore.Models.ViewModels.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Orders
{
    public class HistoryModel
    {
        public string OrderedOn { get; set; }

        public string Username { get; set; }

        public decimal TotalPrice { get; set; }

        public VisualizeBooktemsModel[] Books { get; set; }
    }
}
