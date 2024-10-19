namespace myhomeapplication.Model
{
    public class BookingWithStatusDto
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Services { get; set; }
        public bool SendPaymentLink { get; set; }
        public decimal Total { get; set; }
        public string StatusName { get; set; }
    }
}
