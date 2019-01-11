using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Models.ViewModels.Authors
{
    public class CreateAuthorModel
    {
        private const string ErrorMessage = "You can use only this symbols[(a-z), (A-Z), (0-9), ( _!?.'`:,-)] and length have to be between 50 and 1000.";

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9 _!?.'`:,-]{50,1000}", ErrorMessage = ErrorMessage)]
        public string Details { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}