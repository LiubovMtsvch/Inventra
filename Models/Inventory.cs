using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectitr.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        public string Title { get; set; }

        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPublic { get; set; }

        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
