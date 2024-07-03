namespace CinemaScout_API.Services
{
    public interface IMovieRepository : IGenricRepository<Movie>
    {
        ICollection<ReadMovieDTO> GetAll(byte genreId = 0);
        ReadMovieDTO? GetById(int id);
        ICollection<ReadMovieDTO> GetAllMovieByGenreId(byte genreId);
        int Update(int id, Movie entity);
    }
}
