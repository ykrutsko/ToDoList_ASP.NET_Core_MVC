using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class Settings : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
