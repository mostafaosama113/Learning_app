namespace Learning_platform.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageOfCertificate { get; set; }   
        public int categoryId {  get; set; }  
    }
}
