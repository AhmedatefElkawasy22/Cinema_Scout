namespace CinemaScout_API.UOW
{
    public interface IUnitOfWork
    {
        public IMovieRepository Movie { get; }
        public IGenreRepository Genre { get; }
    }
}
