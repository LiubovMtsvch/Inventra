using CourseProjectitr.Models;

namespace CourseProjectitr.Models
{
    public class CreateItemViewModel
    {
        public int InventoryId { get; set; }

        public string Title { get; set; }
        public string InventoryNumber { get; set; }

        public List<InventoryField> InventoryFields { get; set; } = new();

        public Dictionary<int, string> FieldValues { get; set; } = new(); // key = InventoryField.Id
    }
}
