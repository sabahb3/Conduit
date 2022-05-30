using AutoMapper;
using Conduit.API.Helper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/articles/{slug}/favorite")]
public class FavoriteArticlesController : ControllerBase
{
    private readonly UserIdentity _identity;
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public FavoriteArticlesController(UserIdentity identity,IArticleRepository articleRepository,IMapper mapper)
    {
        _identity = identity;
        _articleRepository = articleRepository;
        _mapper = mapper;
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> FavoriteArticle(string slug)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        var isExist = await _identity.IsExisted(username);
        if (!isExist) return NotFound();
        var article=await _articleRepository.GetArticle(slug);
        if (article == null) return NotFound();
        await _articleRepository.FavoriteArticle(username, article.Id);
        await _articleRepository.Save();
        var articleToReturn = await PrepareArticle(article);
        return Ok(articleToReturn);
    }
    [NonAction]
    public async Task<ArticleToReturnDto> PrepareArticle(Articles article)
    {
        var articleToReturn = _mapper.Map<ArticleToReturnDto>(article);
        articleToReturn.TagList = (await _articleRepository.GetTags(article.Id) as List<string>)!;
        articleToReturn.FavoritesCount = await _articleRepository.CountWhoFavoriteArticle(article.Id);
        var user = await _articleRepository.GetAuthor(article.Id);
        articleToReturn.Author = (await _identity.PrepareProfile(HttpContext.User.Identity,user!.Username))!;
        articleToReturn.Favorited = await IsFavorited(article.Id);
        return articleToReturn;
    }

    [NonAction]
    public async Task<bool> IsFavorited(int articleId)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return false;
        return await _articleRepository.DoesFavoriteArticle(username!, articleId);
    }
}