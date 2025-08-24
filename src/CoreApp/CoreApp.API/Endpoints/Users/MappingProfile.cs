using AutoMapper;

namespace CoreApp.API.Endpoints.Users;

public class MappingProfile : Profile
{
    public MappingProfile() => CreateMap<Domain.Person, UserResponse>(MemberList.None);
}
