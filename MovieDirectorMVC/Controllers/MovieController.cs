using Microsoft.AspNetCore.Mvc;
using MovieDirectorMVC.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MovieDirectorMVC.Controllers
{
    public class MovieController : Controller
    {
        private readonly HttpClient _client;
        private readonly string apiBaseUrl = "http://localhost:5230/api/movie";

        public MovieController()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync(apiBaseUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<List<Movie>>(json);
                return View(movies);
            }
            return View(new List<Movie>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiBaseUrl, content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Something went wrong");
            return View(movie);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(json);

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.Id)
                return BadRequest();

            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{apiBaseUrl}/{id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Update failed.");
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{apiBaseUrl}/{id}");
            return RedirectToAction("Index");
        }



    }
}