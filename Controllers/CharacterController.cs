// CharacterController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarvelTest.Data;
using MarvelTest.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Printing;
using MarvelTest.Services;

namespace MarvelTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly MarvelDBContext _context;

        public CharacterController(MarvelDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacters(string id, string name, string comicId, int pageIndex = 1, int pageSize = 10)
        {
            try 
            {
                var query = _context.Characters.AsQueryable();

                if (!string.IsNullOrEmpty(id))
                {
                    query = query.Where(c => c.id == id);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(c => c.name.Contains(name));
                }

                if (!string.IsNullOrEmpty(comicId))
                {
                    query = query.Where(c => c.comics.Any(comic => comic.id == comicId));
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


