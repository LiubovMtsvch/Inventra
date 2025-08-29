using CourseProjectitr.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } // ← это должно быть
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
