using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace University.MVC.Models.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(16, 100)]
        public int Age { get; set; }

        [Required(ErrorMessage = "Group is required")]
        [StringLength(50)]
        public string Group { get; set; } = string.Empty;

        public int LastGrade { get; set; }
        public double AverageScore { get; set; }
        public List<Guid> EnrollmentIds { get; set; } = new();
    }
}