using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myhomeapplication.Data;
using myhomeapplication.Model;

namespace myhomeapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/bookings/5
        [HttpGet("{user}")]
        public async Task<ActionResult<List<Booking>>> GetBooking(string user)
        {
            
            var bookings = await _context.Bookings
                                         .Where(b => b.User == user)
                                         .OrderByDescending(b => b.Id)
                                         .ToListAsync();

            // Check if the list is empty
            if (bookings == null || !bookings.Any())
            {
                return NotFound(); 
            }

            return Ok(bookings); 
        }
        [HttpGet("byid/{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking([FromBody] Booking booking)
        {
            try
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);

                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
                // Log error for better insight
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Find and delete related records in booking_service
            var relatedServices = _context.Bookings.Where(bs => bs.Id == id);
            _context.Bookings.RemoveRange(relatedServices);

            // Delete the booking itself
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
