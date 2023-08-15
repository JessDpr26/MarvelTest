// CharacterController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarvelTest.Data;
using MarvelTest.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Printing;

namespace MarvelTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicController : ControllerBase
    {
        private readonly MarvelDBContext _context;

        public ComicController(MarvelDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComic(string id, string title, string characterId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Comics.AsQueryable();

                if (!string.IsNullOrEmpty(id))
                {
                    query = query.Where(c => c.id == id);
                }

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(c => c.title.Contains(title));
                }

                if (!string.IsNullOrEmpty(characterId))
                {
                    query = query.Where(c => c.characters.Any(charater => charater.id == characterId));
                }

                var totalCount = await query.CountAsync();
                var characters = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                return Ok(new
                {
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Data = characters
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Ocurrió un error: {ex.Message}"
                });
            }
        }
    }
}


