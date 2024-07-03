
using CinemaScout_API.UOW;
using Microsoft.EntityFrameworkCore;

namespace CinemaScout_API.Services
{
    public class GenricRepository<T> : IGenricRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenricRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Delete(int id)
        {
            T? entity = null;
            if (typeof(T) == typeof(Genre))
            {
                if (id < byte.MinValue || id > byte.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(id), "The id must be between 0 and 255 for Genre.");
                }

                byte genreId = (byte)id;
                entity = _context.Set<T>().Find(genreId);
            }
            else
            {
                entity = _context.Set<T>().Find(id);
            }

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return _context.SaveChanges();
            }

            return 0;
        }


        public int Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            return _context.SaveChanges();
        }

    }
}
