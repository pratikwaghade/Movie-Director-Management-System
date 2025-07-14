namespace MovieDirectorMVC.Models.ViewModels
{
    public class AssignDirectorViewModel
    {
        public List<int> MovieIds { get; set; } = new List<int>();

        public List<int> DirectorIds { get; set; } = new List<int>();

        public List<Movie> Movies { get; set; } = new List<Movie>();

        public List<Director> Directors { get; set; } = new List<Director>();
    }
}
