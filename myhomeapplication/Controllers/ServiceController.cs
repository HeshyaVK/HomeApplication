using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myhomeapplication.Data;
using myhomeapplication.Model;

namespace myhomeapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly AppDbContext _context;
       // private readonly string _imageUploadPath;
        private readonly ILogger _logger;
        private readonly string _imageUploadPath = @"E:\2024kaizen\myhomeapplication\myhomeapplication\Images\";
        public ServiceController(AppDbContext context, IConfiguration configuration, ILogger<ServiceController> logger)
        {
            _context = context;
          //  _imageUploadPath = configuration["ImageUploadPath"];
            _logger = logger;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult<Service>> PutService(int id, [FromForm] ServiceWithFileDtos serviceDto)
        //{
        //    // Retrieve the existing service from the database
        //    var existingService = await _context.Services.FindAsync(id);

        //    if (existingService == null)
        //    {
        //        // Return a 404 if the service does not exist
        //        return NotFound();
        //    }

        //    // Update the service details
        //    existingService.Name = serviceDto.Name;
        //    existingService.Description = serviceDto.Description;
        //    existingService.Price = serviceDto.Price;
        //    existingService.Category=serviceDto.Category;
        //    existingService.Type = serviceDto.Type;

        //    if (serviceDto.File != null && serviceDto.File.Length > 0)
        //    {
        //        // Ensure the directory exists
        //        Directory.CreateDirectory(_imageUploadPath);

        //        var fileName = Path.GetFileName(serviceDto.File.FileName); // Get the file name
        //        var filePath = Path.Combine(_imageUploadPath, fileName); // Combine path and file name

        //        // Save the file to the specified location
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await serviceDto.File.CopyToAsync(stream);
        //        }

        //        // Update the image field of the existing service
        //        existingService.Image = fileName; // Store only the file name or relative path
        //    }

        //    // Save changes to the database
        //    _context.Entry(existingService).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return Ok(existingService);
        //}


        //[HttpPost]
        //public async Task<ActionResult<Service>> PostService([FromForm] ServiceWithFileDtos serviceDto)
        //{
        //    if (serviceDto.File != null && serviceDto.File.Length > 0)
        //    {
        //        Directory.CreateDirectory(_imageUploadPath); // Ensure the directory exists

        //        var fileName = Path.GetFileName(serviceDto.File.FileName); // Get the file name
        //        var filePath = Path.Combine(_imageUploadPath, fileName); // Combine path and file name

        //        // Save the file to the specified location
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await serviceDto.File.CopyToAsync(stream);
        //        }

        //        // Create a Service instance from the DTO data
        //        var service = new Service
        //        {
        //            Name = serviceDto.Name,
        //            Description = serviceDto.Description,
        //            Category = serviceDto.Category,
        //            Image = fileName, // Store only the file name or relative path
        //            Type=serviceDto.Type,
        //            Price=serviceDto.Price,

        //        };

        //        _context.Services.Add(service);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
        //    }
        //    else
        //    {
        //        // Handle case when file is not provided or is empty
        //        ModelState.AddModelError("File", "File is required.");
        //        return BadRequest(ModelState);
        //    }
        //}
        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> PutService(int id, [FromForm] ServiceWithFileDtos serviceDto)
        {
            // Retrieve the existing service from the database
            var existingService = await _context.Services.FindAsync(id);

            if (existingService == null)
            {
                // Return a 404 if the service does not exist
                return NotFound();
            }

            // Update the service details
            existingService.Name = serviceDto.Name;
            existingService.Description = serviceDto.Description;
            existingService.Price = serviceDto.Price;
            existingService.Category = serviceDto.Category;
            existingService.Type = serviceDto.Type;

            if (serviceDto.File != null && serviceDto.File.Length > 0)
            {
                // Ensure the directory exists
                if (!Directory.Exists(_imageUploadPath))
                {
                    Directory.CreateDirectory(_imageUploadPath);
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(serviceDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await serviceDto.File.CopyToAsync(stream);
                }

                // Define the path to copy the file to wwwroot/images
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }

                var finalFilePath = Path.Combine(wwwRootPath, uniqueFileName);
                System.IO.File.Copy(tempFilePath, finalFilePath, true);

                // Update the image field of the existing service
                existingService.Image = Path.Combine("images", uniqueFileName); // Store relative path
            }

            // Save changes to the database
            _context.Entry(existingService).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingService);
        }
        [HttpPost]
        public async Task<ActionResult<Service>> PostService([FromForm] ServiceWithFileDtos serviceDto)
        {
            if (serviceDto.File != null && serviceDto.File.Length > 0)
            {
                // Ensure the directory exists
                if (!Directory.Exists(_imageUploadPath))
                {
                    Directory.CreateDirectory(_imageUploadPath);
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(serviceDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await serviceDto.File.CopyToAsync(stream);
                }

                // Define the path to copy the file to wwwroot/images
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }

                var finalFilePath = Path.Combine(wwwRootPath, uniqueFileName);
                System.IO.File.Copy(tempFilePath, finalFilePath, true);

                // Create a Service instance from the DTO data
                var service = new Service
                {
                    Name = serviceDto.Name,
                    Description = serviceDto.Description,
                    Category = serviceDto.Category,
                    Type = serviceDto.Type,
                    Price = serviceDto.Price,
                    Image = Path.Combine("images", uniqueFileName) // Store relative path
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
            }
            else
            {
                // Handle case when file is not provided or is empty
                ModelState.AddModelError("File", "File is required.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            // Optionally delete the image file
            if (!string.IsNullOrEmpty(service.Image))
            {
                var filePath = Path.Combine(_imageUploadPath, service.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    

}
}


