using CourseProjectitr.Models;

public class Item
{
    public int Id { get; set; } 
    public string Title { get; set; }
    public string InventoryNumber { get; set; }

    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }
}
