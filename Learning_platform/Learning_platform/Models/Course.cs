namespace Learning_platform.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageOfCertificate { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public Category Category { get; set; }
    }
}
