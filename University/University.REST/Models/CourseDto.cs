namespace University.REST.Models
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }

        // ID викладача, який читає цей курс (зв'язок 1:N)
        public Guid? TeacherId { get; set; }
    }
}