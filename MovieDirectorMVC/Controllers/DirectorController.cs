using Microsoft.AspNetCore.Mvc;
using MovieDirectorMVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace MovieDirectorMVC.Controllers
{
    public class DirectorController : Controller
    {
        private readonly HttpClient _client;

        private readonly string apiBaseUrl = "http://localhost:5230/api/director";

        public DirectorController(HttpClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync(apiBaseUrl);

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var directors = JsonConvert.DeserializeObject<List<Director>>(json);

                return View(directors);
            }

            return View(new List<Director>());
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Director dir)
        {
            var json = JsonConvert.SerializeObject(dir);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(apiBaseUrl, content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Something went wrong.");
            return View(dir);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{apiBaseUrl}/{id}");

            if(!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var director = JsonConvert.DeserializeObject<Director>(json);

            return View(director);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Director dir)
        {
            if(id != dir.Id)
            {
                return BadRequest();
            }

            var json = JsonConvert.SerializeObject(dir);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{apiBaseUrl}/{id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Update failed.");
            return View(dir);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{apiBaseUrl}/{id}");
            return RedirectToAction("Index");
        }
    }
}
