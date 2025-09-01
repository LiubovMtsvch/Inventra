
namespace CourseProjectitr.Models
{
    public class ItemFieldValue
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int InventoryFieldId { get; set; }
        public InventoryField InventoryField { get; set; }

        public string? Value { get; set; }
    }

}
