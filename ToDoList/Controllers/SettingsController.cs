using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize(Roles = "admin")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Settings viewModel = new Settings()
            {
                JobPriorities = await _context.JobPriorities.ToListAsync<JobPriority>(),
                JobStatuses = await _context.JobStatuses.ToListAsync<JobStatus>()
            };
            return View("Index", viewModel);
        }
    }
}
