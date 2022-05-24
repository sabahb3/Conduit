using AutoMapper;
using Conduit.API.Credentials;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegistrationCredentials,Users>();
        CreateMap<Users, UserForReturningDto>().ForMember(
            des=>des.Image,
            opt=>opt.MapFrom(src=>src.ProfilePicture)
            );
    }
}