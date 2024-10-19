using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myhomeapplication.Data;
using myhomeapplication.Model;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization; // Include to use IConfiguration

namespace myhomeapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        //private readonly string _imageUploadPath;
        private readonly ILogger _logger;
        private readonly string _imageUploadPath = @"E:\2024kaizen\myhomeapplication\myhomeapplication\Images\";

        public CategoriesController(AppDbContext context, IConfiguration configuration, ILogger<CategoriesController> logger)
        {
             _context = context;
            //_imageUploadPath = configuration["ImageUploadPath"];
            _logger = logger;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        
        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromForm] CategoryWithFileDto categoryDto)
        {
            // Create a new Category object
            var newCategory = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                // Initialize other properties as needed
            };

            if (categoryDto.File != null && categoryDto.File.Length > 0)
            {
                // Ensure the directory exists
                if (!Directory.Exists(_imageUploadPath))
                {
                    Directory.CreateDirectory(_imageUploadPath);
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(categoryDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await categoryDto.File.CopyToAsync(stream);
                }

                // Define the path to copy the file to wwwroot/images
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }

                var finalFilePath = Path.Combine(wwwRootPath, uniqueFileName);
                System.IO.File.Copy(tempFilePath, finalFilePath, true);

                // Update the image field of the new category
                newCategory.Image = Path.Combine("images", uniqueFileName); // Store relative path
            }

            // Add the new category to the context and save changes to the database
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = newCategory.Id }, newCategory);
        }


        //[HttpPost]
        //public async Task<ActionResult<Category>> PostCategory([FromForm] CategoryWithFileDto categoryDto)
        //{
        //    if (categoryDto.File != null && categoryDto.File.Length > 0)
        //    {
        //        Directory.CreateDirectory(_imageUploadPath); // Ensure the directory exists

        //        var fileName = Path.GetFileName(categoryDto.File.FileName); // Get the file name
        //        var filePath = Path.Combine(_imageUploadPath, fileName); // Combine path and file name

        //        // Save the file to the specified location
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await categoryDto.File.CopyToAsync(stream);
        //        }

        //        // Here you create a Category instance from the DTO data
        //        var category = new Category
        //        {
        //            Name = categoryDto.Name,
        //            Description = categoryDto.Description,
        //            Image = fileName // Store only the file name or relative path
        //        };

        //        _context.Categories.Add(category);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        //    }
        //    else
        //    {
        //        // Handle case when file is not provided or is empty
        //        ModelState.AddModelError("File", "File is required.");
        //        return BadRequest(ModelState);
        //    }
        //}


        // PUT: api/categories/5
        //[HttpPut("{id}")]
        //public async Task<ActionResult<Category>> PutCategory(int id, [FromForm] CategoryWithFileDto categoryDto)
        //{
        //    // Retrieve the existing category from the database
        //    var existingCategory = await _context.Categories.FindAsync(id);

        //    if (existingCategory == null)
        //    {
        //        // Return a 404 if the category does not exist
        //        return NotFound();
        //    }

        //    // Update the category details
        //    existingCategory.Name = categoryDto.Name;
        //    existingCategory.Description = categoryDto.Description;

        //    if (categoryDto.File != null && categoryDto.File.Length > 0)
        //    {
        //        // Ensure the directory exists
        //        Directory.CreateDirectory(_imageUploadPath);

        //        var fileName = Path.GetFileName(categoryDto.File.FileName); // Get the file name
        //        var filePath = Path.Combine(_imageUploadPath, fileName); // Combine path and file name

        //        // Save the file to the specified location
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await categoryDto.File.CopyToAsync(stream);
        //        }

        //        // Update the image field of the existing category
        //        existingCategory.Image = fileName; // Store only the file name or relative path
        //    }

        //    // Save changes to the database
        //    _context.Entry(existingCategory).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return Ok(existingCategory);
        //}


        // DELETE: api/categories/5
        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> PutCategory(int id, [FromForm] CategoryWithFileDto categoryDto)
        {
            // Retrieve the existing category from the database
            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
            {
                // Return a 404 if the category does not exist
                return NotFound();
            }

            // Update the category details
            existingCategory.Name = categoryDto.Name;
            existingCategory.Description = categoryDto.Description;

            if (categoryDto.File != null && categoryDto.File.Length > 0)
            {
                // Ensure the directory exists
                if (!Directory.Exists(_imageUploadPath))
                {
                    Directory.CreateDirectory(_imageUploadPath);
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(categoryDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                // Save the file to the specified location
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await categoryDto.File.CopyToAsync(stream);
                }

                // Define the path to copy the file to wwwroot/images
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(wwwRootPath))
                {
                    Directory.CreateDirectory(wwwRootPath);
                }

                var finalFilePath = Path.Combine(wwwRootPath, uniqueFileName);
                System.IO.File.Copy(tempFilePath, finalFilePath, true);

                // Update the image field of the existing category
                existingCategory.Image = Path.Combine("images", uniqueFileName); // Store relative path

                // Optionally delete the old image if needed
                if (!string.IsNullOrEmpty(existingCategory.Image))
                {
                    var oldFilePath = Path.Combine(_imageUploadPath, existingCategory.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            // Save changes to the database
            _context.Entry(existingCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingCategory);
        }



        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Optionally delete the image file
            if (!string.IsNullOrEmpty(category.Image))
            {
                var filePath = Path.Combine(_imageUploadPath, category.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Also delete from wwwroot/images
                var wwwRootImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.Image);
                if (System.IO.File.Exists(wwwRootImagePath))
                {
                    System.IO.File.Delete(wwwRootImagePath);
                }
            }

            // Remove the category from the database
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
