//using CourseProjectitr.Models.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CourseProjectitr.Controllers
//{
//    public class UserController : Controller
//    {
//        [Authorize]
//        public IActionResult Profile()
//        {
//            var model = new DashboardViewModel
//            {
//                OwnedInventories = new List<InventorySummaryViewModel>(),
//                EditableInventories = new List<InventorySummaryViewModel>()
//            };
//            return View(model);
//        }
//    }

//}
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CourseProjectitr.Data;
using CourseProjectitr.Models;
using CourseProjectitr.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var ownedInventories = await _context.Inventories
            .Where(i => i.OwnerId == userId)
            .Select(i => new InventorySummaryViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Category = i.Category,
                Description = i.Description,
                IsPublic = i.IsPublic,
                CreatedAt = i.CreatedAt,
                OwnerName = i.OwnerName,
                ImageUrl = i.ImageUrl,
                Tags = i.Tags.ToList(),
                //переименовать на айди предмета 
                NumberPrefix = i.NumberPrefix

            })
            .ToListAsync();

        var model = new DashboardViewModel
        {
            OwnedInventories = ownedInventories
        };

        return View(model);
    }
}
