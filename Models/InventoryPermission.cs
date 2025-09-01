using System.ComponentModel.DataAnnotations;

namespace CourseProjectitr.Models
{
    public class InventoryPermission
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        [Required]
        public string UserId { get; set; } 

        public bool CanEdit { get; set; } = false;
        public bool CanComment { get; set; } = false;
        public bool CanDelete { get; set; } = false;
    }

}
