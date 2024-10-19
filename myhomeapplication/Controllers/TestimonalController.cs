using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myhomeapplication.Data;
using myhomeapplication.Model;

namespace myhomeapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonalController : ControllerBase
    {
        private readonly AppDbContext _context;
        //private readonly string _imageUploadPath;
        private readonly ILogger _logger;
        private readonly string _imageUploadPath = @"E:\2024kaizen\myhomeapplication\myhomeapplication\Images\";
        public TestimonalController(AppDbContext context, IConfiguration configuration, ILogger<TestimonalController> logger)
        {
            _context = context;
            //_imageUploadPath = configuration["ImageUploadPath"];
            _logger = logger;
        }
        // GET: api/Testimonals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Testimonal>>> GetTestimonals()
        {
            return await _context.Testimonals.ToListAsync();
        }

        // GET: api/Testimonals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Testimonal>> GetTestimonal(int id)
        {
            var testimonal = await _context.Testimonals.FindAsync(id);

            if (testimonal == null)
            {
                return NotFound();
            }

            return testimonal;
        }

        [HttpPost]
        public async Task<ActionResult<Testimonal>> PostTestimonal([FromForm] TestimonalWithFileDto testimonalDto)
        {
            // Create a new Testimonal object
            var newTestimonal = new Testimonal
            {
                Title = testimonalDto.Title,
                Content = testimonalDto.Content,
                Username = testimonalDto.Username,
                Designation = testimonalDto.Designation,
                Rating = testimonalDto.Rating,
                // Initialize other properties as needed
            };

            if (testimonalDto.File != null && testimonalDto.File.Length > 0)
            {
                // Ensure the directory exists
                if (!Directory.Exists(_imageUploadPath))
                {
                    Directory.CreateDirectory(_imageUploadPath);
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(testimonalDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await testimonalDto.File.CopyToAsync(stream);
                }

                // Define the path to copy the file to wwwroot/images
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }

                var finalFilePath = Path.Combine(wwwRootPath, uniqueFileName);
                System.IO.File.Copy(tempFilePath, finalFilePath, true);

                // Update the image field of the new Testimonal
                newTestimonal.Image = Path.Combine("images", uniqueFileName); // Store relative path
            }

            // Add the new testimonal to the context and save changes to the database
            _context.Testimonals.Add(newTestimonal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTestimonal), new { id = newTestimonal.Id }, newTestimonal);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Testimonal>> PutTestimonal(int id, [FromForm] TestimonalWithFileDto testimonalDto)
        {
            // Retrieve the existing testimonial from the database
            var existingTestimonal = await _context.Testimonals.FindAsync(id);

            if (existingTestimonal == null)
            {
                // Return a 404 if the testimonial does not exist
                return NotFound();
            }

            // Update the testimonial details
            existingTestimonal.Title = testimonalDto.Title;
            existingTestimonal.Content = testimonalDto.Content;
            existingTestimonal.Username = testimonalDto.Username;
            existingTestimonal.Designation = testimonalDto.Designation;

            if (testimonalDto.File != null && testimonalDto.File.Length > 0)
            {
                // Ensure the directory exists
                if (!Directory.Exists(_imageUploadPath))
                {
                    Directory.CreateDirectory(_imageUploadPath);
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(testimonalDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await testimonalDto.File.CopyToAsync(stream);
                }

                // Define the path to copy the file to wwwroot/images
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }

                var finalFilePath = Path.Combine(wwwRootPath, uniqueFileName);
                System.IO.File.Copy(tempFilePath, finalFilePath, true);

                // Update the image field of the existing testimonial
                existingTestimonal.Image = Path.Combine("images", uniqueFileName); // Store relative path

                // Optionally delete the old image if needed
                if (!string.IsNullOrEmpty(existingTestimonal.Image))
                {
                    var oldFilePath = Path.Combine(_imageUploadPath, existingTestimonal.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            // Save changes to the database
            _context.Entry(existingTestimonal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingTestimonal);
        }


        // PUT: api/Testimonals/5
        //[HttpPut("{id}")]
        //public async Task<ActionResult<Testimonal>> PutTestimonal(int id, [FromForm] TestimonalWithFileDto testimonalDto)
        //{
        //    // Retrieve the existing testimonial from the database
        //    var existingTestimonal = await _context.Testimonals.FindAsync(id);

        //    if (existingTestimonal == null)
        //    {
        //        // Return a 404 if the testimonial does not exist
        //        return NotFound();
        //    }

        //    // Update the testimonial details
        //    existingTestimonal.Title = testimonalDto.Title;
        //    existingTestimonal.Content = testimonalDto.Content;
        //    existingTestimonal.Username = testimonalDto.Username;
        //    existingTestimonal.Designation = testimonalDto.Designation;


        //    if (testimonalDto.File != null && testimonalDto.File.Length > 0)
        //    {
        //        // Ensure the directory exists
        //        Directory.CreateDirectory(_imageUploadPath);

        //        var fileName = Path.GetFileName(testimonalDto.File.FileName); // Get the file name
        //        var filePath = Path.Combine(_imageUploadPath, fileName); // Combine path and file name

        //        // Save the file to the specified location
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await testimonalDto.File.CopyToAsync(stream);
        //        }

        //        // Update the image field of the existing testimonial
        //        existingTestimonal.Image = fileName; // Store only the file name or relative path
        //    }

        //    // Save changes to the database
        //    _context.Entry(existingTestimonal).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return Ok(existingTestimonal);
        //}

        // DELETE: api/Testimonals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonal(int id)
        {
            var testimonal = await _context.Testimonals.FindAsync(id);
            if (testimonal == null)
            {
                return NotFound();
            }

            // Optionally delete the image file
            if (!string.IsNullOrEmpty(testimonal.Image))
            {
                var filePath = Path.Combine(_imageUploadPath, testimonal.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Testimonals.Remove(testimonal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestimonalExists(int id)
        {
            return _context.Testimonals.Any(e => e.Id == id);
        }
    }
}
