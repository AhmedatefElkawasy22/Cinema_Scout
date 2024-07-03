


using CinemaScout_API.UOW;

namespace CinemaScout_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        //private readonly IGenricRepository<Genre> _genreRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenresController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            //return Ok(_genreRepo.GetAll());
            return Ok(_unitOfWork.Genre.GetAll());
        }
        [HttpGet("{id:int}", Name = "GetGenreByIdRoute")]
        public IActionResult GetById(int id)
        {
            //var genre = _genreRepo.GetById(id);
            var genre = _unitOfWork.Genre.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }
        [HttpPost]
        public IActionResult Add(GenreDTO data)
        {
            if (ModelState.IsValid)
            {
                var add = _mapper.Map<Genre>(data);
                //int res = _genreRepo.Insert(add);
                int res = _unitOfWork.Genre.Insert(add);
                if (res != 0)
                {
                    return Ok(new { data, message = "Successfully added new genre" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to add new genre" });
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Model validation failed", errors });
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] GenreDTO dTO)
        {
            if (ModelState.IsValid)
            {
                var GenreUpdate = _mapper.Map<Genre>(dTO);
                //int res = _genreRepo.Update(id, GenreUpdate);
                int res = _unitOfWork.Genre.Update(id, GenreUpdate);
                int ID = id;
                GenreUpdate.Id = (byte)id;
                if (res > 0)
                {
                    string? url = Url.Link("GetGenreByIdRoute", new { id = ID });
                    return Ok(new { url, message = "Successfully update", GenreUpdate });
                }
                else
                    return BadRequest(new { message = "Failed to update genre" });
            }
            var errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { message = "Model validation failed", errors });
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            //int res = _genreRepo.Delete(id);
            int res = _unitOfWork.Genre.Delete(id);
            if (res == 0)
            {
                return NotFound($"not found Genre with this id : {id}");
            }
            return Ok("Successfully Deleted");
        }
    }
}
