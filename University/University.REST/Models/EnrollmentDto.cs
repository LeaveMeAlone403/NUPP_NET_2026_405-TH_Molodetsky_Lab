namespace University.REST.Models
{
    public class EnrollmentDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public int? Grade { get; set; } 
    }
}