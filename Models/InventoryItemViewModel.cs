namespace CourseProjectitr.Models
{
    public class InventoryItemViewModel
    {
        public int InventoryId { get; set; }
        public string InventoryTitle { get; set; }

        public string Title { get; set; }
        public string InventoryNumber { get; set; }

        public List<CustomFieldValue> CustomFields { get; set; }
    }

    public class CustomFieldValue
    {
        public int FieldId { get; set; }
        public string FieldTitle { get; set; }
        public string FieldType { get; set; } // "text", "textarea", "number", "url", "bool"
        public string Description { get; set; }
        public string Value { get; set; }
    }
}
