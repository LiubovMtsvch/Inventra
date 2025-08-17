//using Microsoft.AspNetCore.Mvc;
//using CourseProjectitr.Models;

//public class InventoryController : Controller
//{
//    // GET: Inventory/Create
//    [HttpGet]
//    public IActionResult Create()
//    {
//        return View("~/Views/User/CreateInventory.cshtml");
//    }

//    // POST: Inventory/Create
//    [HttpPost]

//    public IActionResult Create(Inventory model)
//    {
//        if (!ModelState.IsValid)
//        {
//            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
//            return Content("Ошибки: " + string.Join(", ", errors));
//        }

//        return RedirectToAction("Profile", "User", new { area = "" });
//    }

//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CourseProjectitr.Models;
using CourseProjectitr.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class InventoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public InventoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Inventory/Create
    [HttpGet]
    public IActionResult Create()
    {
        var categories = _context.Categories
         .Select(c => new SelectListItem
         {
             Value = c.Name, // или c.Id, если ты хранишь ID
             Text = c.Name
         })
         .ToList();

        ViewBag.Categories = categories;

        return View("~/Views/User/CreateInventory.cshtml");
    }

    // POST: Inventory/Create
    [HttpPost]
    public async Task<IActionResult> Create(Inventory model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Content("Ошибки: " + string.Join(", ", errors));
        }

        // Привязываем текущего пользователя
        model.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        model.OwnerName = User.Identity.Name;
        model.CreatedAt = DateTime.UtcNow;

        _context.Inventories.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Profile", "User");
    }
}

