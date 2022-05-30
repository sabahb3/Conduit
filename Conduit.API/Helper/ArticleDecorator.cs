using System.Security.Principal;
using AutoMapper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;

namespace Conduit.API.Helper;

public static class ArticleDecorator
{
    public static async Task<ArticleToReturnDto> PrepareArticle(this Articles article, IMapper mapper,IArticleRepository articleRepository, UserIdentity userIdentity,IIdentity? identity)
    {
        var articleToReturn = mapper.Map<ArticleToReturnDto>(article);
        articleToReturn.TagList = (await articleRepository.GetTags(article.Id) as List<string>)!;
        articleToReturn.FavoritesCount = await articleRepository.CountWhoFavoriteArticle(article.Id);
        var author = await articleRepository.GetAuthor(article.Id);
        articleToReturn.Author = (await userIdentity.PrepareProfile(identity,author!.Username))!;
        var loggedUser = userIdentity.GetLoggedUser(identity);
        articleToReturn.Favorited = loggedUser != null && await articleRepository.DoesFavoriteArticle(loggedUser, article.Id);
        return articleToReturn;
    }
}