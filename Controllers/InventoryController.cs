using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CourseProjectitr.Models;
using CourseProjectitr.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

[Authorize]
public class InventoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public InventoryController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult SearchTags(string term)
    {
        var tags = _context.Tags
            .Where(t => t.Name.StartsWith(term))
            .Select(t => t.Name)
            .ToList();

        return Json(tags);
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
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return Content("Ошибки: " + string.Join(", ", errors));
        }

 
        model.NumberPrefix = await GenerateNumberPrefix();

        
        if (!string.IsNullOrEmpty(model.TagsJson))
        {
            var tagNames = JsonSerializer.Deserialize<List<string>>(model.TagsJson);
            model.Tags = new List<Tag>();

            foreach (var name in tagNames)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);

                if (existingTag != null)
                {
                    model.Tags.Add(existingTag);
                }
                else
                {
                    var newTag = new Tag { Name = name };
                    model.Tags.Add(newTag);
                }
            }
        }

        // Остальные поля
        model.OwnerName = HttpContext.Session.GetString("CurrentUserName") ?? "Unknown";

        model.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Google ID

        model.CreatedAt = DateTime.UtcNow;

        model.OwnerEmail = User.FindFirstValue(ClaimTypes.Email)?? HttpContext.Session.GetString("CurrentUserEmail")?? "unknown@example.com";




        _context.Inventories.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Profile", "User");
    }

    // Метод генерации NumberPrefix
    private async Task<string> GenerateNumberPrefix()
    {
        var count = await _context.Inventories.CountAsync();
        var year = DateTime.UtcNow.Year;
        return $"INV-{year}-{count + 1:D3}";
    }


    public async Task<IActionResult> Details(int id)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Tags)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (inventory == null)
            return NotFound();

        return View(inventory);
    }


    [HttpGet]
    public async Task<IActionResult> SelectDelete([FromQuery] List<int> ids)
    {
        Console.WriteLine("SelectDelete GET: " + string.Join(",", ids)); // ← добавь это

        var inventories = await _context.Inventories
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();

        return View("Delete", inventories);
    }


    [HttpPost, ActionName("SelectDelete")]
    [ValidateAntiForgeryToken]


    public async Task<IActionResult> SelectDeleteConfirmed(List<int> ids)


    {
        var inventories = await _context.Inventories
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();

        _context.Inventories.RemoveRange(inventories);
        await _context.SaveChangesAsync();

        return RedirectToAction("Profile", "User");
    }


    [Authorize]
    public IActionResult Edit([FromQuery] List<int> ids)
    {
        if (ids == null || !ids.Any())
            return NotFound();

        var inventories = _context.Inventories
            .Where(i => ids.Contains(i.Id))
            .ToList();

        var categories = _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            })
            .ToList();

        ViewBag.Categories = categories;

        return View(inventories); 
    }


    // POST: Inventory/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(List<Inventory> inventories)
    {
        if (inventories == null || !inventories.Any())
            return BadRequest();

        foreach (var item in inventories)
        {
            var existing = _context.Inventories.Find(item.Id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.Description = item.Description;
                existing.Category = item.Category;
                existing.ImageUrl = item.ImageUrl;
                existing.IsPublic = item.IsPublic;
            }
        }

        _context.SaveChanges();
        return RedirectToAction("Profile", "User");
    }
}







