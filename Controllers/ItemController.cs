using CourseProjectitr.Data;
using CourseProjectitr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectitr.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int inventoryId)
        {
            var inventory = _context.Inventories
                .Include(i => i.InventoryFields)
                .FirstOrDefault(i => i.Id == inventoryId);

            if (inventory == null) return NotFound();

            var vm = new CreateItemViewModel
            {
                InventoryId = inventory.Id,
                InventoryFields = inventory.InventoryFields.ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateItemViewModel model)
        {
            if (!ModelState.IsValid)
            {

                model.InventoryFields = _context.InventoryFields
                    .Where(f => f.InventoryId == model.InventoryId)
                    .ToList();

                return View(model);
            }

            var item = new Item
            {
                Title = model.Title,
                InventoryNumber = model.InventoryNumber,
                InventoryId = model.InventoryId
            };

            _context.Items.Add(item);
            _context.SaveChanges(); 

            foreach (var kvp in model.FieldValues)
            {
                var value = new ItemFieldValue
                {
                    ItemId = item.Id,
                    InventoryFieldId = kvp.Key,
                    Value = kvp.Value
                };
                _context.ItemFieldValues.Add(value);
            }

            _context.SaveChanges();

            return RedirectToAction("Details", "Inventory", new { id = model.InventoryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items
                .Include(i => i.FieldValues)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return NotFound();

            
            _context.ItemFieldValues.RemoveRange(item.FieldValues);

    
            _context.Items.Remove(item);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Inventory", new { id = item.InventoryId });
        }

    }



}
