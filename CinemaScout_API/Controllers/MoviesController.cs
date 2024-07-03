
using CinemaScout_API.UOW;

namespace CinemaScout_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MoviesController : ControllerBase
    {
        //private readonly IGenricRepository<Movie> _movieRepo;
        //private readonly IGenreRepository _extraGenreRepo;
        //private readonly IMovieRepository _extraMovieRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _map;

        public MoviesController(IMapper map, IUnitOfWork unitOfWork)
        {
            _map = map;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //ICollection<Movie> movies = _unitOfWork.Movie.GetAll();
            //ICollection<ReadMovieDTO> readMovies = _map.Map<ICollection<ReadMovieDTO>>(movies);
            return Ok(_unitOfWork.Movie.GetAll());
        }
        [HttpGet("{id:int}", Name = "GetMovieById")]
        public IActionResult GetById(int id)
        {
            ReadMovieDTO? movie = _unitOfWork.Movie.GetById(id);
            if (movie == null)
                return NotFound($"Not found movie with this id : {id}");

            return Ok(movie);
        }
        [HttpGet("GetMovieByGenreId/{GenreId:int}")]
        public IActionResult GetMovieByGenreId([FromRoute] int GenreId)
        {
            if (!(_unitOfWork.Genre.Found(GenreId)))
                return NotFound($"Genre not found with this ID: {GenreId}");

            byte id = (byte)GenreId;
            return Ok(_unitOfWork.Movie.GetAllMovieByGenreId(id).Select(a => new { a.Id, a.Title, a.StoreLine, a.Rate, a.Year, a.Genre, a.Poster }));
        }

        [HttpPost]
        public IActionResult Add([FromForm] MovieDTO dTO)
        {
            if (dTO.Poster == null)
                return BadRequest("Poster is Required !");

            var validationResult = CheckExtension(dTO.Poster);
            if (!validationResult.IsSuccess)
                return BadRequest(validationResult.ErrorMessage);
            if (dTO.Poster.Length > SettingsPosters.MaxAllowPosterSize)
                return BadRequest("MAX Allowed size for Image is 1MB !");
            bool IsVaildGanreID = _unitOfWork.Genre.Found(dTO.GenreId);
            if (!IsVaildGanreID)
                return BadRequest($"Not Found Genre with this id {dTO.GenreId}");

            if (ModelState.IsValid)
            {
                Movie movie = _map.Map<Movie>(dTO);
                int res = _unitOfWork.Movie.Insert(movie);
                if (res == 0)
                {
                    return BadRequest("a problem happened");
                }
                return Ok("The movie has been added successfully");
            }
            var Errors = ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { message = "Model validation failed", Errors });
        }
        [HttpPut("{id:int}")]
        public IActionResult Edit([FromRoute] int id, [FromForm] MovieDTO dTO)
        {
            if (dTO.Poster != null)
            {
                var validationResult = CheckExtension(dTO.Poster);
                if (!validationResult.IsSuccess)
                    return BadRequest(validationResult.ErrorMessage);
                if (dTO.Poster.Length > SettingsPosters.MaxAllowPosterSize)
                    return BadRequest("MAX Allowed size for Image is 1MB !");
            }

            bool IsVaildGanreID = _unitOfWork.Genre.Found(dTO.GenreId);
            if (!IsVaildGanreID)
                return BadRequest($"Not Found Genre with this id {dTO.GenreId}");


            if (ModelState.IsValid)
            {
                Movie movie = _map.Map<Movie>(dTO);
                int res = _unitOfWork.Movie.Update(id, movie);
                if (res == 0)
                {
                    return BadRequest(new { message = "Failed to update genre" });
                }
                int ID = id;
                string? url = Url.Link("GetMovieById", new { id = ID });
                movie.Id = ID;
                ReadMovieDTO read = _map.Map<ReadMovieDTO>(movie);
                read.Genre = _unitOfWork.Genre.GetById(movie.Id);
                return Ok(new { url, message = "Successfully update", read });
            }
            var Errors = ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { message = "Model validation failed", Errors });
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            int res = _unitOfWork.Movie.Delete(id);
            if (res == 0)
            {
                return NotFound($"not found movie with this id : {id}");
            }
            return Ok("Successfully remove");
        }

        private (bool IsSuccess, string ErrorMessage) CheckExtension(IFormFile file)
        {
            if (!SettingsPosters.AllowExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                return (false, "Only .jpg and .png images are allowed.");
            }
            return (true, null);
        }
    }
}
