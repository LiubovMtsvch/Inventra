using CourseProjectitr.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var recentInventories = await _context.Inventories
            .Where(i => i.IsPublic)
            .OrderByDescending(i => i.CreatedAt)
            .Take(10)
            .Include(i => i.Tags)
            .ToListAsync();

        return View(recentInventories);
    }
}
