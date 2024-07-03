namespace CinemaScout_API.Services
{
    public class GenreRepository : GenricRepository<Genre>, IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<Genre> GetAll()
        {
            return _context.Genres.AsNoTracking().ToList();
        }

        public Genre? GetById(int id)
        {
            if (id < byte.MinValue || id > byte.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The id must be between 0 and 255 for Genre.");
            }
            byte genreId = (byte)id;
            return _context.Genres.AsNoTracking().SingleOrDefault(d => d.Id == genreId);
        }

        public int Update(int id, Genre entity)
        {
            Genre? genre = GetById(id);
            if (genre != null)
            {
                genre.Name = entity.Name;
                _context.Genres.Update(genre);
                return _context.SaveChanges();
            }
            return 0;
        }


        public bool Found(int id)
        {
            return _context.Genres.Any(d => d.Id == id);
        }


    }
}
