using System;
using System.Collections.Generic;

namespace CourseProjectitr.Models.ViewModels
{
    public class InventorySummaryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public string Category { get; set; }
        public List<Tag> Tags { get; set; } = new();

        public bool IsPublic { get; set; }
        public string OwnerName { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        //переименовать на айди предмета 
        public string NumberPrefix { get; set; }


    }
}
