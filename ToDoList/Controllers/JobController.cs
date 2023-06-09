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

        public JobController(AppDbContext context)
        {
            _context = context;
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
                    User = job.CreationUser,
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

                jobInDB.Name = job.Name;
                jobInDB.Description = job.Description;
                jobInDB.CreatedDate = job.CreatedDate;
                jobInDB.DueDate = job.DueDate;
                jobInDB.JobPriorityId = job.JobPriorityId;
                jobInDB.JobStatusId = job.JobStatusId;
                jobInDB.CreationUser = job.CreationUser;
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
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
            }

            viewModel.User = User.Identity.Name;
            viewModel.Job = new Job();
            viewModel.PageTitle = "New Task";
            return View("JobForm", viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            NewJobViewModel viewModel = new NewJobViewModel();
            Job job;
            try
            {
                job = await _context.Jobs.SingleOrDefaultAsync<Job>(j => j.Id == id);
                if (job is null)
                    return NotFound();

                viewModel.Job = job;
                viewModel.JobPriorities = await _context.JobPriorities.ToListAsync<JobPriority>();
                viewModel.JobStatuses = await _context.JobStatuses.ToListAsync<JobStatus>();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = ex.Message });
            }

            viewModel.User = job.CreationUser;
            viewModel.PageTitle = "Edit Task";

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
