using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TaskManager.Enumerations;

namespace TaskManager.Dto
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority? Priority { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PriorityList { get; set; } = new List<SelectListItem>();
    }
}
