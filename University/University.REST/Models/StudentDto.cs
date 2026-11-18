namespace University.REST.Models
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Group { get; set; } = string.Empty;
        public int LastGrade { get; set; }
        public double AverageScore { get; set; }
        public List<Guid> EnrollmentIds { get; set; } = new();
    }
}