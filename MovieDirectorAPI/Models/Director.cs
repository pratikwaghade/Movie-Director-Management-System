namespace MovieDirectorAPI.Models
{
    public class Director
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Nationality { get; set; }

        public ICollection<MovieDirector>? MovieDirectors { get; set; }

    }
}
