using CourseProjectitr.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("Tags")]
public class TagsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TagsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("Suggest")]
    public async Task<IActionResult> Suggest(string query)
    {
        var tags = await _context.Tags
            .Where(t => t.Name.Contains(query))
            .Select(t => t.Name)
            .Take(10)
            .ToListAsync();

        return Json(tags);
    }

}
