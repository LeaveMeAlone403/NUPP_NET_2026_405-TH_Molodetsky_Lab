namespace University.REST.Models
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public int AcademicExperienceYears { get; set; }
        public Guid? ClassroomId { get; set; }

        // Список ID курсів, які викладає цей викладач
        public List<Guid> CourseIds { get; set; } = new();
    }
}