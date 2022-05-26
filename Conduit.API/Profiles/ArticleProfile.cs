using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Profiles;

public class ArticleProfile : Profile
{
    private readonly IArticleRepository _articleRepository;

    public ArticleProfile(IArticleRepository articleRepository, ITagRepository tagRepository)
    {
        _articleRepository = articleRepository;
        CreateMap<Articles, ArticleToReturnDto>().ForMember(
            des=>des.Slug,
            opt=>opt.MapFrom(src=>src.Title)
            )
            .ForMember(des=>des.FavoritesCount,
                opt=>
                    opt.MapFrom(src=>_articleRepository.CountWhoFavoriteArticle(src.Id))
                )
            ;
    }
}