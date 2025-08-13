using System.Collections.Generic;

namespace CourseProjectitr.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<InventoryViewModel> OwnedInventories { get; set; } = new();
        public List<InventoryViewModel> EditableInventories { get; set; } = new();
    }
}
