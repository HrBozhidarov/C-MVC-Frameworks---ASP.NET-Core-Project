using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models.ViewModels.Comments
{
    public class RattingModel
    {
        public int FiveStartCount { get; set; }

        public int FourStartCount { get; set; }

        public int ThreeStartCount { get; set; }

        public int TwoStartCount { get; set; }

        public int OneStartCount { get; set; }
    }
}
