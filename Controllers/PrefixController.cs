using CourseProjectitr.Data;
using CourseProjectitr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ItemController : Controller
{
    private readonly ApplicationDbContext _context;

    public ItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<IActionResult> Create(Item model)
    //{
    //    if (!ModelState.IsValid)
    //        return View(model);

    //    model.InventoryNumber = await GenerateInventoryNumber(model.InventoryId);

    //    _context.Items.Add(model);
    //    await _context.SaveChangesAsync();

    //    return RedirectToAction("Details", "Inventory", new { id = model.InventoryId });
    //}

    //private async Task<string> GenerateInventoryNumber(int inventoryId)
    //{
    //    var count = await _context.Items
    //    .Where(i => i.InventoryId == inventoryId)
    //    .CountAsync();



    //    var prefix = ""; 
    //    var year = DateTime.UtcNow.Year;

    //    return $"{prefix}-{year}-{count + 1:D3}";
    //}
    private async Task<string> GenerateInventoryNumber(int inventoryId)
    {
        var random = new Random();

        string letters = new string(Enumerable.Range(0, 2)
            .Select(_ => (char)random.Next('A', 'Z' + 1))
            .ToArray());

        int digits = random.Next(0, 1000);

        return $"{letters}_{digits:D3}";
    }

}
