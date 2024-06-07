using BusinessObjects.Entities;
using AutoMapper;
using BusinessObjects.DTOs;

namespace HostelandAuthorization.Helper {
    public class MappingProfiles : Profile {
        public MappingProfiles() {
            // AUTH
            CreateMap<UserLoginRequestDto, ApplicationUser>();
            CreateMap<ApplicationUser, GetPersonalUserDto>();
        }
    }
}
