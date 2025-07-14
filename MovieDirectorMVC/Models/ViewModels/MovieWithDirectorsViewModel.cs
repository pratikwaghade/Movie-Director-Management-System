using MovieDirectorMVC.DTO;
using Newtonsoft.Json;

namespace MovieDirectorMVC.Models.ViewModels
{
    public class MovieWithDirectorsViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        
        [JsonProperty("directorList")]
        public List<DirectorDto>? DirectorList { get; set; }
    }
}
