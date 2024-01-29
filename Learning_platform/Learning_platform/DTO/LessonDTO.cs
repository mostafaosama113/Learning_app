namespace Learning_platform.DTO
{
    public class LessonDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile VideoFile { get; set; }
        public int Price { get; set; }
        public int Vote { get; set; }
        public int CourseId { get; set; }
    }
}
