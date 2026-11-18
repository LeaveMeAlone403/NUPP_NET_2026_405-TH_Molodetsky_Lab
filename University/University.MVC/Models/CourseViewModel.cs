using System;
using System.ComponentModel.DataAnnotations;

namespace University.MVC.Models.ViewModels
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Credits must be between 1 and 100")]
        public int Credits { get; set; }

        public Guid? TeacherId { get; set; }
    }
}