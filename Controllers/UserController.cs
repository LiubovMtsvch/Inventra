using CourseProjectitr.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectitr.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public IActionResult Profile()
        {
            var model = new DashboardViewModel
            {
                OwnedInventories = new List<InventoryViewModel>(),
                EditableInventories = new List<InventoryViewModel>()
            };
            return View(model);
        }
    }

}
