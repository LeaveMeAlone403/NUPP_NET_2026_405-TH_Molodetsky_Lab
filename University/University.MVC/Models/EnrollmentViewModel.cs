using System;

namespace University.MVC.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public int? Grade { get; set; }
    }
}