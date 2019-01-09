using BookStore.Models.ViewModels.Books;
using BookStore.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStore.Models.ViewModels.Orders
{
    public class CreateOrderModel
    {
        [Required]
        public string UserId { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
