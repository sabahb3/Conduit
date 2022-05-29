using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class ArticleProfile : Profile
{

    public ArticleProfile()
    {
        CreateMap<Articles, ArticleToReturnDto>().ForMember(
            des=>des.Slug,
            opt=>opt.MapFrom(src=>src.Title)
            )
            .ForMember(des=>des.CreatedAt,
                opt=>opt.MapFrom(src=>src.Date)
                )
            .ForMember(des=>des.UpdatedAt,
                opt=>opt.MapFrom(src=>src.UpdatingDate)
            );
        CreateMap<ArticleForCreation, Articles>().ForMember(
            des=>des.ArticlesTags,
            opt=>
                opt.MapFrom<ArticleTagsResolver>()
            );
        CreateMap<ArticleForUpdatingDto,Articles>();
    }
}