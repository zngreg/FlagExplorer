using AutoMapper;
using FlagExplorerApp.Services.Models;

namespace FlagExplorerApp.Services.Configs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CountryInfo, Country>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Common))
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => src.Flag));

            CreateMap<CountryInfo, CountryDetails>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Common))
                .ForMember(dest => dest.Capital, opt => opt.MapFrom(src => string.Join(", ", src.Capital)))
                .ForMember(dest => dest.Flag, opt => opt.MapFrom(src => src.Flag))
                .ForMember(dest => dest.Population, opt => opt.MapFrom(src => src.Population));
        }
    }
}