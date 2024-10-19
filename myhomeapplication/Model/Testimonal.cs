namespace myhomeapplication.Model
{
    public class Testimonal
    {
        public int Id { get; set; }  // Unique ID for the review
        public string Image { get; set; }  // Path to the image of the reviewer
        public string Title { get; set; }  // Title of the review
        public int Rating { get; set; }  // Rating given in the review (out of 5)
        public string Content { get; set; }  // Review content or description
        public string Username { get; set; }  // Name of the user who left the review
        public string Designation { get; set; }
    }
}
