using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myhomeapplication.Data;
using myhomeapplication.Model;

namespace myhomeapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticPageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _imageUploadPath = @"E:\2024kaizen\myhomeapplication\myhomeapplication\Images\";
        private readonly ILogger _logger;

        public StaticPageController(AppDbContext context, IConfiguration configuration, ILogger<StaticPageController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/StaticPages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaticPage>>> GetStaticPages()
        {
            return await _context.StaticPages.ToListAsync();
        }

        // GET: api/StaticPages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaticPage>> GetStaticPage(int id)
        {
            var staticPage = await _context.StaticPages.FindAsync(id);

            if (staticPage == null)
            {
                return NotFound();
            }

            return staticPage;
        }
        [HttpPost]
        public async Task<ActionResult<StaticPage>> PostStaticPage([FromForm] StaticpageWithFileDto staticPageDto)
        {
            var newStaticPage = new StaticPage
            {
                Title = staticPageDto.Title
            };

            if (staticPageDto.File != null && staticPageDto.File.Length > 0)
            {
                Directory.CreateDirectory(_imageUploadPath);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(staticPageDto.File.FileName)}";
                var filePath = Path.Combine(_imageUploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await staticPageDto.File.CopyToAsync(stream);
                }

                newStaticPage.Banner = Path.Combine("images", uniqueFileName); // Store relative path
            }

            _context.StaticPages.Add(newStaticPage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStaticPage), new { id = newStaticPage.Id }, newStaticPage);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<StaticPage>> PutStaticPage(int id, [FromForm] StaticpageWithFileDto staticPageDto)
        {
            var existingStaticPage = await _context.StaticPages.FindAsync(id);

            if (existingStaticPage == null)
            {
                return NotFound();
            }

            existingStaticPage.Title = staticPageDto.Title;
            existingStaticPage.MetaTile = staticPageDto.MetaTile;
            existingStaticPage.Content= staticPageDto.Content;
            existingStaticPage.MetaDescription= staticPageDto.MetaDescription;

            if (staticPageDto.File != null && staticPageDto.File.Length > 0)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(staticPageDto.File.FileName)}";
                var tempFilePath = Path.Combine(_imageUploadPath, uniqueFileName);

                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await staticPageDto.File.CopyToAsync(stream);
                }

                existingStaticPage.Banner = Path.Combine("images", uniqueFileName); // Store relative path
            }

            _context.Entry(existingStaticPage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingStaticPage);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaticPage(int id)
        {
            var staticPage = await _context.StaticPages.FindAsync(id);
            if (staticPage == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(staticPage.Banner))
            {
                var filePath = Path.Combine(_imageUploadPath, staticPage.Banner);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                var wwwRootImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", staticPage.Banner);
                if (System.IO.File.Exists(wwwRootImagePath))
                {
                    System.IO.File.Delete(wwwRootImagePath);
                }
            }

            _context.StaticPages.Remove(staticPage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StaticPageExists(int id)
        {
            return _context.StaticPages.Any(e => e.Id == id);
        }
    }
}
