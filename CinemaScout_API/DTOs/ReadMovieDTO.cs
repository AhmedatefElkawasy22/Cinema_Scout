namespace CinemaScout_API.DTOs
{
    public class ReadMovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoreLine { get; set; }
        public Genre Genre { get; set; }
        public byte[] Poster { get; set; }
    }
}
