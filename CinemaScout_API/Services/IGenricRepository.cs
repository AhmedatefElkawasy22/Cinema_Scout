namespace CinemaScout_API.Services
{
    public interface IGenricRepository < T > where T : class
    {
       
        int Delete(int id);
        int Insert(T entity);
    }
}
