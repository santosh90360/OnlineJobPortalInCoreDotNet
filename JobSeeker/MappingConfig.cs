using AutoMapper;
using JobSeeker.Models;
using JobSeeker.Models.Dto;

namespace JobSeeker
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<RegistrationDto, Registration>().ReverseMap();
                config.CreateMap<SkillDto, Skill>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
