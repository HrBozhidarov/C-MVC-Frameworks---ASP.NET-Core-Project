﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Models.ViewModels.Books
{
    public class BookCreateModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(40)]
        public string Title { get; set; }

        [Range(1,double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{13}")]
        public string Isbn { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [Required]
        [MinLength(100)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public IList<string> Authors { get; set; } = new List<string>();

        public IList<string> Categories { get; set; } = new List<string>();
    }
}
