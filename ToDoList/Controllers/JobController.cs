﻿using Microsoft.AspNetCore.Authorization;
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
    }
}
