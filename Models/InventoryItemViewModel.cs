using System.ComponentModel.DataAnnotations;

namespace CourseProjectitr.Models
{
    public class InventoryItemViewModel
    {
        public int InventoryId { get; set; }

        public string? InventoryTitle { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Inventory Number is required")]
        public string InventoryNumber { get; set; }

        public List<CustomFieldValue>? CustomFields { get; set; }

        public DateTime? CreatedAt { get; set; } 
        public string? CreatedBy { get; set; }   
        public string? CreatorEmail { get; set; } 
    }
}

public class CustomFieldValue
    {
        public int FieldId { get; set; }
        public string FieldTitle { get; set; }
        public string FieldType { get; set; } 
        public string Description { get; set; }
        public string Value { get; set; }
    }

