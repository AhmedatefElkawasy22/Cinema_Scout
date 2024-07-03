
namespace CinemaScout_API.Services
{
    public class MovieRepository : GenricRepository <Movie> , IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _map;


        public MovieRepository(ApplicationDbContext context, IMapper map):base(context)
        {
            _context = context;
            _map = map;
        }

        public ICollection<ReadMovieDTO> GetAll(byte genreId = 0)
        {
            ICollection<Movie> movies= _context.Movies.Include(g => g.Genre)
                .Where(w => w.GenreId == genreId || genreId == 0)
                .AsNoTracking().ToList();
            //ICollection<ReadMovieDTO> readMovies = 
            return _map.Map<ICollection<ReadMovieDTO>>(movies);
        }
        public ReadMovieDTO? GetById(int id)
        {
            Movie? movie= _context.Movies.AsNoTracking().Include(d => d.Genre).SingleOrDefault(d => d.Id == id);
            return _map.Map<ReadMovieDTO>(movie);
        }

        public int Update(int id, Movie entity)
        {
            Movie? movie = _context.Movies.Find(id);
            if (movie != null)
            {
                byte[] temp = movie.Poster;

                _map.Map(entity, movie);

                if (entity.Poster == null || entity.Poster.Length == 0)
                    movie.Poster = temp;

                _context.Movies.Update(movie);
                return _context.SaveChanges();
            }
            return 0;
        }
        public ICollection<ReadMovieDTO> GetAllMovieByGenreId(byte genreId)
        {
            return GetAll(genreId);
        }

    }
}
