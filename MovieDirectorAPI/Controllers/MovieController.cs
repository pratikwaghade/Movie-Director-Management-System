using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDirectorAPI.Data;
using MovieDirectorAPI.DTO;
using MovieDirectorAPI.Models;
using System.Reflection;

namespace MovieDirectorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _context.Movies.ToListAsync();

            var result = new List<MovieDto>();

            foreach (var movie in movies)
            {
                result.Add(new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    ReleaseYear = movie.ReleaseYear,
                    MovieType = movie.MovieType
                });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            var dto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                MovieType = movie.MovieType
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(MovieDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                ReleaseYear = dto.ReleaseYear,
                MovieType = dto.MovieType
            };

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            dto.Id = movie.Id;

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, MovieDto dto)
        {
            if(id != dto.Id)
            {
                return BadRequest("Movie ID in URL and body does not match.");
            }

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound("Movie id not found");
            }

            movie.Title = dto.Title;
            movie.ReleaseYear = dto.ReleaseYear;
            movie.MovieType = dto.MovieType;

            await _context.SaveChangesAsync();

            return Ok("Movie updated succesfully");

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if(movie == null)
            {
                return NotFound($"Movie with ID {id} not found.");
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok($"Movie with ID {id} deleted successfully.");
        }



    }
}
