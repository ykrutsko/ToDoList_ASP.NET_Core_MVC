using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Settings
    {
        public int PriorityId { get; set; }

        [Required]
        public string PriorityName { get; set; }

        public int StatusId { get; set; }
        [Required]
        public string StatusName { get; set; }

        public List<JobPriority> JobPriorities { get; set; }
        public List<JobStatus> JobStatuses { get; set; }
    }
}
