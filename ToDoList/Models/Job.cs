using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Creation date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Deadline")]
        public DateTime DueDate { get; set; }

        public string CreationUser { get; set; }

        public bool IsDone { get; set; }

        [Display(Name = "Priority")]
        public int JobPriorityId { get; set; }

        public JobPriority JobPriority { get; set; }

        [Display(Name = "Satatus")]
        public int JobStatusId { get; set; }

        public JobStatus JobStatus { get; set; }
    }
}
