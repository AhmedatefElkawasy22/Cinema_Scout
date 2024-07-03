namespace CinemaScout_API.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _map;

        public IMovieRepository Movie { get; }
        public IGenreRepository Genre { get; }

        public UnitOfWork(ApplicationDbContext context, IMapper map)
        {
            _context = context;
            _map = map;
            Movie = new MovieRepository(_context, _map);
            Genre = new GenreRepository(_context);
        }
    }
}
