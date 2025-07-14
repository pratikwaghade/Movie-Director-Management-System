using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MovieDirectorAPI.Data;
using MovieDirectorAPI.DTO;
using MovieDirectorAPI.Models;

namespace MovieDirectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieDirectorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieDirectorController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("assign")]
        public async Task<IActionResult> AssignDirectors([FromBody] LinkMovieDirectorDto dto)
        {
            var movie = await _context.Movies.FindAsync(dto.MovieId);

            if (movie == null)
                return NotFound();

            foreach(var directorId in dto.DirectorIds)
            {
                bool exists = await _context.MovieDirectors
                    .AnyAsync(md => md.MovieId == dto.MovieId && md.DirectorId == directorId);

                if (!exists)
                {
                    var movieDirector = new MovieDirector
                    {
                        MovieId = dto.MovieId,
                        DirectorId = directorId
                    };
                    _context.MovieDirectors.Add(movieDirector);
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Directors assigned to movie.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLinks()
        {
            // Step 1: Load all MovieDirector records with their Movie and Director details
            var allLinks = await _context.MovieDirectors
                .Include(md => md.Movie)
                .Include(md => md.Director)
                .ToListAsync();

            // Step 2: Group the data by MovieId
            var result = allLinks
                .GroupBy(md => md.Movie) // Group by Movie object
                .Select(group => new MovieWithDirectorsDto
                {
                    MovieId = group.Key.Id,
                    Title = group.Key.Title,
                    ReleaseYear = group.Key.ReleaseYear,
                    DirectorList = group.Select(md => new DirectorDto
                    {
                        Id = md.Director.Id,
                        Name = md.Director.Name,
                        Nationality = md.Director.Nationality

                    }).ToList()
                })
                .ToList();

            // Step 3: Return the final list
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateDirectors([FromBody] LinkMovieDirectorDto dto)
        {
            var movie = await _context.Movies.FindAsync(dto.MovieId);
            if(movie == null)
            {
                return NotFound("Movie not found.");
            }

            // Remove all existing links
            var existingLinks = _context.MovieDirectors.Where(md => md.MovieId == dto.MovieId);
            _context.MovieDirectors.RemoveRange(existingLinks);

            //Add new links
            foreach(var directorId in dto.DirectorIds)
            {
                _context.MovieDirectors.Add(new MovieDirector
                {
                    MovieId = dto.MovieId,
                    DirectorId = directorId,
                });
            }
            
            await _context.SaveChangesAsync();
            return Ok("Movie-director assignments updated.");
        }


        [HttpDelete("unassign")]
        public async Task<IActionResult> Unassign([FromQuery] int movieId, [FromQuery] int directorId)
        {
            var link = await _context.MovieDirectors
                .FirstOrDefaultAsync(md => md.MovieId == movieId && md.DirectorId == directorId);

            if (link == null)
                return NotFound("Link not found.");

            _context.MovieDirectors.Remove(link);
            await _context.SaveChangesAsync();

            return Ok("Director unassigned from movie.");
        }

    }
}
