using CourseProjectitr.Models;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string InventoryNumber { get; set; }

    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }

    public ICollection<ItemFieldValue> FieldValues { get; set; } = new List<ItemFieldValue>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public string? CreatorEmail { get; set; }

    public string? CustomId { get; set; } 

}
