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
             Value = c.Name, 
             Text = c.Name
         })
         .ToList();

        ViewBag.Categories = categories;

        return View("~/Views/User/CreateInventory.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Create(Inventory model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                })
                .ToList();

            return View("~/Views/User/CreateInventory.cshtml", model); 
        }


        model.NumberPrefix = await GenerateNumberPrefix();
        model.OwnerName = HttpContext.Session.GetString("CurrentUserName") ?? "Unknown";
        model.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        model.CreatedAt = DateTime.UtcNow;
        model.OwnerEmail = User.FindFirstValue(ClaimTypes.Email) ?? HttpContext.Session.GetString("CurrentUserEmail") ?? "unknown@example.com";


        if (!string.IsNullOrEmpty(model.TagsJson))
        {
            var tagNames = JsonSerializer.Deserialize<List<string>>(model.TagsJson);
            var tags = new List<Tag>();

            foreach (var name in tagNames)
            {
                var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
                if (existingTag != null)
                {
                    tags.Add(existingTag);
                }
                else
                {
                    var newTag = new Tag { Name = name };
                    _context.Tags.Add(newTag);
                    await _context.SaveChangesAsync(); // сохранить новый тег
                    tags.Add(newTag);
                }
            }

            model.Tags = tags;
        }


        // 📌 Сохраняем инвентаризацию
        _context.Inventories.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Profile", "User");
    }
    //private async Task<string> GenerateNumberPrefix()
    //{
    //    var count = await _context.Inventories.CountAsync();
    //    var year = DateTime.UtcNow.Year;
    //    return $"INV-{year}-{count + 1:D3}";
    //}
    private async Task<string> GenerateNumberPrefix()
    {
        var random = new Random();

        // Две случайные заглавные буквы
        string letters = new string(Enumerable.Range(0, 2)
            .Select(_ => (char)random.Next('A', 'Z' + 1))
            .ToArray());

        // Три случайные цифры от 000 до 999
        int digits = random.Next(0, 1000);

        return $"{letters}_{digits:D3}";
    }



    //private async Task<string> GenerateItemNumber(int inventoryId)
    //{
    //    var count = await _context.Items.CountAsync(i => i.InventoryId == inventoryId);
    //    return $"ITEM-{inventoryId}-{count + 1:D3}";
    //}

    private string GenerateCustomId(int inventoryId)
    {
        var inventory = _context.Inventories.FirstOrDefault(i => i.Id == inventoryId);
        if (inventory == null) return "UNKNOWN";

        var count = _context.Items.Count(i => i.InventoryId == inventoryId);
        var prefix = inventory.NumberPrefix ?? "INV";
        return $"{prefix}_{count + 1:D3}";

    }


    public IActionResult AddItem(int id)
    {
        var inventory = _context.Inventories.Find(id);
        if (inventory == null) return NotFound();

        var viewModel = new InventoryItemViewModel
        {
            InventoryId = inventory.Id,
            InventoryTitle = inventory.Title
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(InventoryItemViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var item = new Item
        {
            Title = model.Title,
            InventoryNumber = model.InventoryNumber,
            InventoryId = model.InventoryId,
            CustomId = GenerateCustomId(model.InventoryId),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = HttpContext.Session.GetString("CurrentUserName") ?? User.Identity?.Name ?? "Unknown",
            CreatorEmail = User.FindFirstValue(ClaimTypes.Email) ?? HttpContext.Session.GetString("CurrentUserEmail") ?? "unknown@example.com"
        };



        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = model.InventoryId });
    }


    [HttpGet]
    public IActionResult EditItem(int id)
    {
        var item = _context.Items.FirstOrDefault(i => i.Id == id);
        if (item == null) return NotFound();

        var editItemModel = new EditItemViewModel
        {
            Id = item.Id,
            InventoryId = item.InventoryId,
            Title = item.Title,
            InventoryNumber = item.InventoryNumber,
            CustomId = item.CustomId
        };

        return View("EditItem", editItemModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditItem(EditItemViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var item = _context.Items.FirstOrDefault(i => i.Id == model.Id);
        if (item == null) return NotFound();

        item.Title = model.Title;
        item.InventoryNumber = model.InventoryNumber;
        item.CustomId = model.CustomId;

        _context.SaveChanges();

        return RedirectToAction("Details", "Inventory", new { id = model.InventoryId });
    }



    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var inventory = await _context.Inventories
        .Include(i => i.Tags)
        .Include(i => i.Items) 
        .FirstOrDefaultAsync(i => i.Id == id);


        if (inventory == null)
            return NotFound();

        return View(inventory);
    }

    [AllowAnonymous]
    public async Task<IActionResult> DetailsForAll(int id)
    {
        var inventory = await _context.Inventories
            .Include(i => i.Items) 
            .Include(i => i.Tags)  
            .FirstOrDefaultAsync(i => i.Id == id);

        return View("DetailsForAll", inventory);
    }



    [HttpGet]
    public async Task<IActionResult> SelectDelete([FromQuery] List<int> ids)
    {
        Console.WriteLine("SelectDelete GET: " + string.Join(",", ids)); 

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







