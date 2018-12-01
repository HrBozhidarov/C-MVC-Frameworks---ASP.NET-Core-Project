using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Models.ViewModels.Authors
{
    public class EditAuthorModel
    {
        private const string ErrorMessage = "You can use only this symbols[(a-z), (A-Z), (0-9), (_!?.`,-)] and length have to be between 50 and 500.";

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9_!?.`,-]{50,500}", ErrorMessage = ErrorMessage)]
        public string Details { get; set; }
    }
}
