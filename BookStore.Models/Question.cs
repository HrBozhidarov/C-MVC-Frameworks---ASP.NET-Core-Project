using System;

namespace BookStore.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Title { get; set; }

        public bool IsSeen { get; set; }
    }
}