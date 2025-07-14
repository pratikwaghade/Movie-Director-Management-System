namespace MovieDirectorAPI.DTO
{
    public class MovieWithDirectorsDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }

        public int ReleaseYear { get; set; }

        public List<DirectorDto> DirectorList { get; set; } = new List<DirectorDto>();

    }
}
