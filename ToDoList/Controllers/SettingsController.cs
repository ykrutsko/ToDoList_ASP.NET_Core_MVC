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

        [HttpPost]
        public async Task<ActionResult> AddNewPriority(Settings model)
        {
            JobPriority jobPriority = await _context.JobPriorities.SingleOrDefaultAsync(m => m.Name == model.PriorityName);

            if (jobPriority == null)
            {
                JobPriority newJobPriority = new JobPriority
                {
                    Name = model.PriorityName
                };
                await _context.JobPriorities.AddAsync(newJobPriority);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "This priority is already exists!";
            };
            return RedirectToAction("Index", TempData);
        }

    }
}
