using AutoMapper;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class CommentsProfile : Profile
{
    public CommentsProfile()
    {
        CreateMap<CommentForCreationDto, Comments>();
        CreateMap<Comments,CommentToReturnDto>()
            .ForMember(des=>des.CreatedAt,
                opt=>opt.MapFrom(src=>src.Date)
                )
            .ForMember(des=>des.UpdatedAt,
                opt=>opt.MapFrom(src=>src.Date)
            )
            
            ;
    }
}