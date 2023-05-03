using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JobController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            AppViewModel newModel = new AppViewModel();
            try
            {
                newModel.Jobs = await _context.Jobs.Include(p => p.JobPriority).Include(s => s.JobStatus).ToListAsync<Job>();
                newModel.JobPriorities = await _context.JobPriorities.ToListAsync<JobPriority>();
                newModel.JobStatuses = await _context.JobStatuses.ToListAsync<JobStatus>();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
            }
            return View("Index", newModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Job job)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewJobViewModel
                {
                    Job = job,
                    JobPriorities = await _context.JobPriorities.ToListAsync(),
                    JobStatuses = await _context.JobStatuses.ToListAsync(),
                    User = job.User,
                    PageTitle = "New job"
                };
                return View("JobForm", viewModel);
            }
            if (job.Id == 0)
            {
                await _context.Jobs.AddAsync(job);
            }
            else
            {
                Job jobInDB = await _context.Jobs.SingleAsync(j => j.Id == job.Id);
                TryValidateModel(jobInDB);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Job");
        }

        public async Task<IActionResult> New()
        {
            NewJobViewModel viewModel = new NewJobViewModel();
            try
            {
                viewModel.JobPriorities = await _context.JobPriorities.ToListAsync<JobPriority>();
                viewModel.JobStatuses = await _context.JobStatuses.ToListAsync<JobStatus>();
                viewModel.User = await _userManager.GetUserAsync(User);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
            }

            viewModel.Job = new Job();
            viewModel.PageTitle = "New Job";

            return View("JobForm", viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            NewJobViewModel viewModel = new NewJobViewModel();
            try
            {
                Job job = await _context.Jobs.SingleOrDefaultAsync<Job>(j => j.Id == id);
                if (job is null)
                    return NotFound();

                viewModel.Job = job;
                viewModel.User = await _userManager.GetUserAsync(User);
                viewModel.JobPriorities = await _context.JobPriorities.ToListAsync<JobPriority>();
                viewModel.JobStatuses = await _context.JobStatuses.ToListAsync<JobStatus>();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
            }
            viewModel.PageTitle = "Edit Job";

            return View("JobForm", viewModel);
        }

        public async Task<IActionResult> Complete(int id)
        {
            Job job = await _context.Jobs.SingleOrDefaultAsync<Job>(j => j.Id == id);
            if (job is null)
                return NotFound();

            job.IsDone = !job.IsDone;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            Job job = await _context.Jobs.SingleOrDefaultAsync<Job>(j => j.Id == id);
            if (job is null)
                return NotFound();

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
