using CourseProjectitr.Models;
using System.Collections.Generic;

namespace CourseProjectitr.ViewModels
{
    public class SearchResultsViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<Inventory> Inventories { get; set; } = new();
        public List<Item> Items { get; set; } = new();
    }
}
