using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Orders
{
    public class IncomeModel
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public decimal Income { get; set; }
    }
}
