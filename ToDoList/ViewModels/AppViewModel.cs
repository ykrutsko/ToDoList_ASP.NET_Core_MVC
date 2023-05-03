using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class AppViewModel
    {
        public Job Job { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
        public JobPriority JobPriority { get; set; }
        public IEnumerable<JobPriority> JobPriorities { get; set; }
        public JobStatus JobStatus { get; set; }
        public IEnumerable<JobStatus> JobStatuses { get; set; }
        public IdentityUser User { get; set; }
    }
}
