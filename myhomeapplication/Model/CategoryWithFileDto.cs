namespace myhomeapplication.Model
{
    public class CategoryWithFileDto
    {
        public string Name { get; set; } 
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
