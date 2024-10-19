namespace myhomeapplication.Model
{
    public class Setting
    {
        public int Id { get; set; }  // Unique identifier for the settings
        public string FooterContent { get; set; }  // Footer content text
        public string Address { get; set; }  // Business address
        public string PhoneNumber { get; set; }  // Contact phone number
        public string Email { get; set; }  // Contact email address
        public string Map { get; set; }  // Embedded map URL
        public string Facebook { get; set; }  // Facebook page URL
        public string Instagram { get; set; }  // Instagram page URL
        public string Twitter { get; set; }  // Twitter profile URL
        public string Youtube { get; set; }  // YouTube channel URL

    }
}
