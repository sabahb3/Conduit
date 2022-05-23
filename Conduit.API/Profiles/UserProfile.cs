using AutoMapper;
using Conduit.API.Credentials;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegistrationCredentials,Users>();
    }
}