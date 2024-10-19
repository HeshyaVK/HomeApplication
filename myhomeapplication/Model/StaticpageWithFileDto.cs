namespace myhomeapplication.Model
{
    public class StaticpageWithFileDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile File { get; set; }
        public string MetaTile { get; set; }
        public string MetaDescription { get; set; }
    }
}
