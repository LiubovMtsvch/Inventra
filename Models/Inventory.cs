using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProjectitr.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string? OwnerEmail { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        public string Title { get; set; }

        [NotMapped]
        public string? TagsJson { get; set; }


        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        public string Category { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsPublic { get; set; }

        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }

      

      


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public string? NumberPrefix { get; set; }
        public ICollection<InventoryPermission> Permissions { get; set; } = new List<InventoryPermission>();





    }
}
