using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class NewJobViewModel : AppViewModel
    {
        public string PageTitle { get; set; }
        public string User { get; set; }
        public string UserId { get; set; }
        public NewJobViewModel() :base()
        {
            
        }
    }
}
