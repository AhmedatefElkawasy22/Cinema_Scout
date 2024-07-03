namespace CinemaScout_API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();

            CreateMap<Movie, ReadMovieDTO>().ReverseMap();

            CreateMap<Movie, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Genre, opt => opt.Ignore())
            .ForMember(dest => dest.Poster, opt =>
                opt.Condition(src => src.Poster != null)).
                ReverseMap().
                ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Genre, opt => opt.Ignore())
            .ForMember(dest => dest.Poster, opt =>
                opt.Condition(src => src.Poster != null));

            CreateMap<Movie, MovieDTO>()
             .ForMember(dest => dest.Poster, opt =>
                 opt.MapFrom(src => src.Poster != null ? ConvertByteArrayToIFormFile(src.Poster) : null))
             .ReverseMap()
             .ForMember(dest => dest.Poster, opt =>
                 opt.MapFrom(src => src.Poster != null ? ConvertIFormFileToByteArray(src.Poster) : null));
        }
        private byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            if (file == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        private IFormFile ConvertByteArrayToIFormFile(byte[] byteArray)
        {
            if (byteArray == null)
                return null;

            var stream = new MemoryStream(byteArray);
            return new FormFile(stream, 0, byteArray.Length, "Poster", "poster.jpg");
        }
    }
}
