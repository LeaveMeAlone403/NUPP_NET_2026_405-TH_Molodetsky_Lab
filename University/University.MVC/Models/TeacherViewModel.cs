using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace University.MVC.Models.ViewModels
{
    public class TeacherViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(20, 100)]
        public int Age { get; set; }

        [Range(0, 1000000)]
        public decimal Salary { get; set; }

        [Range(0, 70)]
        public int AcademicExperienceYears { get; set; }

        public List<Guid> CourseIds { get; set; } = new();
    }
}