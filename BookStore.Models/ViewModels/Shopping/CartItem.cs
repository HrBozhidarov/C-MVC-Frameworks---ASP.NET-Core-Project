using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Shopping
{
    public class CartItem
    {
        public int BookId { get; set; }

        public int Quantity { get; set; }
    }
}
