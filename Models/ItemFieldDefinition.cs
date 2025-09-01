namespace CourseProjectitr.Models
{
    public class ItemFieldDefinition
    {
        public Guid Id { get; set; }

        public Guid InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public string Key { get; set; } // Уникальный ключ
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // "text", "textarea", "number", "checkbox", "link"
        public bool ShowInTable { get; set; }
    }

}
