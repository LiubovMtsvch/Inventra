using System.Collections.Generic;

namespace CourseProjectitr.Models.ViewModels
{
    public class DashboardViewModel

    {
        public List<InventorySummaryViewModel> OwnedInventories { get; set; } = new();
        public List<InventorySummaryViewModel> EditableInventories { get; set; } = new();
        public List<Inventory> AccessibleInventories { get; set; }

    }
}
