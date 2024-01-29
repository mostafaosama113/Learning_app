namespace Learning_platform.DTO
{
    public class AddCourseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageOfCertificate { get; set; }
        public int Category_Id { get; set; }
    }
}
