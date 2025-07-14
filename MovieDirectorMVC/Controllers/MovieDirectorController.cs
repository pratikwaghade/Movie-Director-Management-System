using Microsoft.AspNetCore.Mvc;
using MovieDirectorMVC.DTO;
using MovieDirectorMVC.Models;
using MovieDirectorMVC.Models.ViewModels;
using Newtonsoft.Json;
using System.Text;
namespace MovieDirectorMVC.Controllers
{
    public class MovieDirectorController : Controller
    {
        private readonly HttpClient _client;
        private readonly string movieApi = "http://localhost:5230/api/movie";
        private readonly string directorApi = "http://localhost:5230/api/director";
        private readonly string assignApi = "http://localhost:5230/api/moviedirector/assign";
        public MovieDirectorController(HttpClient client)
        {
            this._client = client;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("http://localhost:5230/api/MovieDirector");
            if (!response.IsSuccessStatusCode)
            {
                return View(new List<MovieWithDirectorsViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var links = JsonConvert.DeserializeObject<List<MovieWithDirectorsViewModel>>(json);


            return View(links);
        }

        [HttpGet]
        public async Task<IActionResult> Assign()
        {
            var vm = new AssignDirectorViewModel();

            // Get Movies
            var movieRes = await _client.GetAsync(movieApi);
            if (movieRes.IsSuccessStatusCode)
            {
                var json = await movieRes.Content.ReadAsStringAsync();
                vm.Movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            }

            // Get Directors
            var dirRes = await _client.GetAsync(directorApi);
            if (dirRes.IsSuccessStatusCode)
            {
                var json = await dirRes.Content.ReadAsStringAsync();
                vm.Directors = JsonConvert.DeserializeObject<List<Director>>(json);
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(AssignDirectorViewModel model)
        {
            var dtoList = new List<LinkMovieDirectorDto>();

            foreach (var movieId in model.MovieIds)
            {
                var dto = new LinkMovieDirectorDto
                {
                    MovieId = movieId,
                    DirectorIds = model.DirectorIds
                };

                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("http://localhost:5230/api/MovieDirector/assign", content);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Unassign(int movieId, int directorId)
        {
            var response = await _client.DeleteAsync($"http://localhost:5230/api/MovieDirector/unassign?movieId={movieId}&directorId={directorId}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Director Unassigned Successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to unassign";
            }

            return RedirectToAction("Index");
        }
    }
}

