namespace myhomeapplication.Model
{
    public class NewBooking
    {
        public int Id { get; set; } // Unique identifier for the booking.
        public DateTime DateTime { get; set; } // Date and time of the booking.
        public string Name { get; set; } // Name of the user.
        public string Email { get; set; } // Email of the user.
        public string Phone { get; set; }
        public string Names { get; set; } // Name of the service.
        public decimal Price { get; set; } // Price of the service.
        public decimal SubTotal { get; set; } // Subtotal of all the services.
        public string Status { get; set; } = string.Empty;

        
        
    }
}
