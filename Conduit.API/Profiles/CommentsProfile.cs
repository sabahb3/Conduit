using AutoMapper;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class CommentsProfile : Profile
{
    public CommentsProfile()
    {
        CreateMap<CommentForCreationDto, Comments>();
    }
}