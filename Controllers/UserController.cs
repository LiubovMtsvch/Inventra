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

    
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

   
        var ownedInventoriesRaw = await _context.Inventories
            .Where(i => i.OwnerId == userId)
            .Include(i => i.Tags)
            .ToListAsync();

        var ownedInventories = ownedInventoriesRaw
            .Select(MapToSummary)
            .ToList();

     
        var editablePermissions = await _context.InventoryPermissions
            .Where(p => p.UserId == userId && p.CanEdit)
            .Include(p => p.Inventory)
                .ThenInclude(inv => inv.Tags)
            .ToListAsync();

        var editableInventories = editablePermissions
            .Select(p => MapToSummary(p.Inventory))
            .ToList();


        var accessiblePermissions = await _context.InventoryPermissions
            .Where(p => p.UserId == userId)
            .Include(p => p.Inventory)
            .ToListAsync();

        var accessibleInventories = accessiblePermissions
            .Select(p => p.Inventory)
            .ToList();

        var model = new DashboardViewModel
        {
            OwnedInventories = ownedInventories,
            EditableInventories = editableInventories,
            AccessibleInventories = accessibleInventories
        };


        return View(model);

    }

    [HttpGet("/api/users/search")]
    public IActionResult SearchUsers(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Ok(new List<object>());

        var loweredQuery = query.ToLower();

        var users = _context.Inventories
        .Where(i =>
            (i.OwnerName != null && i.OwnerName.ToLower().Contains(loweredQuery)) ||
            (i.OwnerEmail != null && i.OwnerEmail.ToLower().Contains(loweredQuery))
        )
        .GroupBy(i => i.OwnerEmail)
        .Select(g => new {
            id = g.First().OwnerId,
            name = g.First().OwnerName,
            email = g.Key,
            value = $"{g.First().OwnerName} ({g.Key})"
        })

        .Take(10)
        .ToList();


        return Ok(users);
    }




    private InventorySummaryViewModel MapToSummary(Inventory inventory)
    {
        return new InventorySummaryViewModel
        {
            Id = inventory.Id,
            Title = inventory.Title,
            Category = inventory.Category,
            Description = inventory.Description,
            IsPublic = inventory.IsPublic,
            CreatedAt = inventory.CreatedAt,
            OwnerName = inventory.OwnerName,
            ImageUrl = inventory.ImageUrl,
            Tags = inventory.Tags?.ToList() ?? new List<Tag>(),
            NumberPrefix = inventory.NumberPrefix
        };

    }
}
