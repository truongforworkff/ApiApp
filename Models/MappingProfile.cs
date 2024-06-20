using AutoMapper;

namespace FptJobBack.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobPosting, JobPostingDtoGet>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name)) // Example mapping for nested property
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Username)); ;
            CreateMap<Users, UserDto>();
            CreateMap<UserProfile, UserProfileCreateDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<Category, CategoryDto>();
        }
    }
}
