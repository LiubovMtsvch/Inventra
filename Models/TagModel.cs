using System.Collections.Generic;

namespace CourseProjectitr.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    }
}
