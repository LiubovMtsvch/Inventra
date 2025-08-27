using CourseProjectitr.Data;
using CourseProjectitr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CourseProjectitr.Controllers
{
    [Authorize]
    public class InventoryAccessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryAccessController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
   
        public async Task<IActionResult> GrantAccess(int inventoryId, string userIds, bool canEdit, bool canComment)
        {
            var parsedIds = JsonSerializer.Deserialize<List<string>>(userIds);
            if (parsedIds == null || !parsedIds.Any())
                return BadRequest("Не выбраны пользователи");

            foreach (var userId in parsedIds)
            {
                var permission = new InventoryPermission
                {
                    InventoryId = inventoryId,
                    UserId = userId,
                    CanEdit = canEdit,
                    CanComment = canComment
                };
                _context.InventoryPermissions.Add(permission);
            }

            await _context.SaveChangesAsync();
            return Ok("Доступ выдан");
        }


        [HttpPost]
        public async Task<IActionResult> RevokeAccess(int inventoryId, string userId)
        {
            var permission = await _context.InventoryPermissions
                .FirstOrDefaultAsync(p => p.InventoryId == inventoryId && p.UserId == userId);

            if (permission != null)
            {
                _context.InventoryPermissions.Remove(permission);
                await _context.SaveChangesAsync();
            }

            return Ok("Доступ удалён");
        }

        [HttpGet]
        public async Task<IActionResult> GetEditableInventories(string userId)
        {
            var editableInventories = await _context.InventoryPermissions
                .Where(p => p.UserId == userId && p.CanEdit)
                .Include(p => p.Inventory)
                .Select(p => p.Inventory)
                .ToListAsync();

            return Ok(editableInventories);
        }

    }

}
