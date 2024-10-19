namespace myhomeapplication.Model
{
    public class Booking
    {
        public int Id { get; set; } // Assuming there's an Id for the appointment.
        public string User { get; set; } // Represents the user who booked the appointment.
        public string AppointmentDate { get; set; } // Represents the date and time of the appointment.
        public string AppointmentTime { get; set; } // Represents the time of the appointment.
        public string Address1 { get; set; } // First address line.
        public string Address2 { get; set; } // Second address line.
        public string City { get; set; } // City for the appointment.
        public string State { get; set; } // State for the appointment.
        public string Pincode { get; set; } // Pincode for the appointment.
        public string Services { get; set; } // Name of the service.        
        public bool SendPaymentLink { get; set; } // Whether to send a payment link to the user.
        public decimal Total { get; set; }

        public int StatusID { get; set; }

        public string OrderId { get; set; }
        public string PayerId { get; set; }
        public string PaymentId { get; set; }
        public string PaymentSource { get; set; }
    }
}
