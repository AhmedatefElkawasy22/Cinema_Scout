namespace CinemaScout_API.Services
{
    public interface IGenreRepository : IGenricRepository<Genre>
    {
        ICollection <Genre> GetAll();
        Genre? GetById(int id);
        int Update(int id,Genre genre);
        bool Found(int id);
    }
}
