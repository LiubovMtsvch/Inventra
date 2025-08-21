using CourseProjectitr.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }



       public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
   

}
