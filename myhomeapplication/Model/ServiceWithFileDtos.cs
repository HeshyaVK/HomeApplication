namespace myhomeapplication.Model
{
    public class ServiceWithFileDtos
    {
      
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
    }
}
