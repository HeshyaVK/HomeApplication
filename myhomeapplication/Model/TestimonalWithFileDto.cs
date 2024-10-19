namespace myhomeapplication.Model
{
    public class TestimonalWithFileDto
    {
       
        public string Title { get; set; }  
        public int Rating { get; set; }  
        public string Content { get; set; }  
        public string Username { get; set; }  
        public string Designation { get; set; }
        public IFormFile File { get; set; }
    }
}
