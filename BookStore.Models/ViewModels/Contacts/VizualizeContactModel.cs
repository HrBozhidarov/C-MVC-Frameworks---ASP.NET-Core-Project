using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Models.ViewModels.Contacts
{
    public class VizualizeContactModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(25)]
        [MaxLength(300)]
        public string Content { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Title { get; set; }

        public bool IsSeen { get; set; }
    }
}
