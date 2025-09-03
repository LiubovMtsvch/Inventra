using CourseProjectitr.Data;
using CourseProjectitr.Models;
using CourseProjectitr.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectitr.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string query)
        {
            query = query?.ToLower().Trim();

            var inventories = await _context.Inventories
                .Include(i => i.Tags)
                .Where(i =>
                    i.IsPublic &&
                    (string.IsNullOrWhiteSpace(query) ||
                     (!string.IsNullOrEmpty(i.Title) && i.Title.ToLower().Contains(query)) ||
                     (!string.IsNullOrEmpty(i.Category) && i.Category.ToLower().Contains(query)) ||
                     (i.Tags != null && i.Tags.Any(t => t.Name.ToLower().Contains(query)))
                    )
                )
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            return View(inventories); 
        }


    }
}

