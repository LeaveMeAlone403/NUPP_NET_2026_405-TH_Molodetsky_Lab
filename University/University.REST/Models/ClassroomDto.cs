namespace University.REST.Models
{
    public class ClassroomDto
    {
        public Guid Id { get; set; }
        public int RoomNumber { get; set; } 
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }
        public Guid? TeacherId { get; set; }
    }
}