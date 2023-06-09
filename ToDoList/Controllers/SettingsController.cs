﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
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
                JobPriorities = await _context.JobPriorities.ToListAsync(),
                JobStatuses = await _context.JobStatuses.ToListAsync()
            };
            return View("Index", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewPriority(Settings model)
        {
            JobPriority jobPriority = await _context.JobPriorities.SingleOrDefaultAsync(x => x.Name == model.PriorityName);

            if (jobPriority == null)
            {
                if(string.IsNullOrEmpty(model.PriorityName))
                    TempData["ErrorPriority"] = "Priority name is required!";
                else
                {
                    JobPriority newJobPriority = new JobPriority
                    {
                        Name = model.PriorityName
                    };
                    await _context.JobPriorities.AddAsync(newJobPriority);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }                
            }
            else
            {
                TempData["ErrorPriority"] = "This priority is already exists!";
            };
            return RedirectToAction("Index", TempData);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewStatus(Settings model)
        {
            JobStatus jobStatus = await _context.JobStatuses.SingleOrDefaultAsync(x => x.Name == model.StatusName);

            if (jobStatus == null)
            {
                if (string.IsNullOrEmpty(model.StatusName))
                    TempData["ErrorStatus"] = "Status name is required!";
                else
                {
                    JobStatus newJobStatus = new JobStatus
                    {
                        Name = model.StatusName
                    };
                    await _context.JobStatuses.AddAsync(newJobStatus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorStatus"] = "This status is already exists!";
            };
            return RedirectToAction("Index", TempData);
        }

        public async Task<ActionResult> DeletePriority(int id)
        {
            JobPriority jobPriority = await _context.JobPriorities.SingleOrDefaultAsync(x => x.Id == id);
            List<Job> jobList = await _context.Jobs.Include(x => x.JobPriority).ToListAsync();
            int priorityListCount = await _context.JobPriorities.CountAsync();

            var Message = string.Empty;
            foreach (var item in jobList)
            {
                if (item.JobPriority.Name == jobPriority.Name)
                {
                    Message = "We have a task with this priority!";
                    return Json(Message);
                }
            }
            if (priorityListCount <= 1)
            {
                Message = "We should have at least one priority!";
                return Json(Message);
            }
            else
            {
                _context.Remove(jobPriority);
                await _context.SaveChangesAsync();
                return Json(Message);
            }
        }

        public async Task<ActionResult> DeleteStatus(int id)
        {
            JobStatus jobStatus = await _context.JobStatuses.SingleOrDefaultAsync(x => x.Id == id);
            List<Job> jobList = await _context.Jobs.Include(x => x.JobStatus).ToListAsync();
            int statusListCount = await _context.JobStatuses.CountAsync();
            var Message = string.Empty;

            foreach (var item in jobList)
            {
                if (item.JobStatus.Name == jobStatus.Name)
                {
                    Message = "We have a task with this status";
                    return Json(Message);
                }
            }
            if (statusListCount <= 1)
            {
                Message = "We should have at least one status!";
                return Json(Message);
            }
            else
            {
                _context.Remove(jobStatus);
                await _context.SaveChangesAsync();
                return Json(Message);
            }
        }
    }
}
