using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDirectorAPI.Data;
using MovieDirectorAPI.DTO;
using MovieDirectorAPI.Models;

namespace MovieDirectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DirectorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDirectors()
        {
            var directors = await _context.Directors.ToListAsync();

            var result = new List<DirectorDto>();

            foreach (var director in directors)
            {
                result.Add(new DirectorDto
                {
                    Id = director.Id,
                    Name = director.Name,
                    Nationality = director.Nationality
                });
            }

            await _context.SaveChangesAsync();
            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var director = await _context.Directors.FindAsync(id);

            if(director == null)
            {
                return NotFound();
            }

            var dto = new DirectorDto
            {
                Id = director.Id,
                Name = director.Name,
                Nationality = director.Nationality
            };

            return Ok(dto);
        }

        [HttpPost]
       public async Task<IActionResult> AddDirector(DirectorDto dto)
        {
            var director = new Director
            {
                Name = dto.Name,
                Nationality = dto.Nationality
            };

            await _context.Directors.AddAsync(director);
            await _context.SaveChangesAsync();

            dto.Id = dto.Id;    

            return Ok(dto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDirector(int id, DirectorDto dto)
        {
            if(id != dto.Id)
            {
                return BadRequest("ID mismatch between URL and body");
            }

            var director = await _context.Directors.FindAsync(id);

            if(director == null)
            {
                return NotFound();
            }

            director.Name = dto.Name;
            director.Nationality = dto.Nationality;

            await _context.SaveChangesAsync();

            return Ok("Director updated succesfully.");
            
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var director = await _context.Directors.FindAsync(id);

            if(director == null)
                return NotFound();

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return Ok("Director deleted successfully.");
        }
    }
}
