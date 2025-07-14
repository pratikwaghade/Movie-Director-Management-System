namespace MovieDirectorAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int ReleaseYear { get; set; }

        public string? MovieType { get; set; }

        // public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();
        public ICollection<MovieDirector>? MovieDirectors { get; set; }
    }
}
