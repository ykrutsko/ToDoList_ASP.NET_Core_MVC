using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize(Roles = "admin")]
    public class Settings : Controller
    {
        private readonly AppDbContext _context;

        public Settings(AppDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
