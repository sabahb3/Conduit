using System.Security.Claims;
using AutoMapper;
using Conduit.API.Helper;
using Conduit.API.ResourceParameters;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;
    private readonly ITagRepository _tagRepository;
    private readonly UserIdentity _identity;

    public ArticlesController(IArticleRepository articleRepository, IMapper mapper, ITagRepository tagRepository,
        UserIdentity identity)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
        _tagRepository = tagRepository;
        _identity = identity;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetArticles([FromQuery] ArticleResourceParameter? articleResourceParameter)
    {
        var articles = await _articleRepository.GetArticles(_articleRepository.Articles, articleResourceParameter);
        var articlesToReturn = await PrepareArticles(articles);
        return Ok(new {articles =articlesToReturn,articlesCount= articlesToReturn.Count()});
    }

    [NonAction]
    public async Task<IEnumerable<ArticleToReturnDto>> PrepareArticles(IEnumerable<Articles> articles)
    {
        var articlesToReturn = new List<ArticleToReturnDto>();
        foreach (var article in articles) articlesToReturn.Add(await PrepareArticle(article));
        return articlesToReturn;
    }

    [NonAction]
    public async Task<ArticleToReturnDto> PrepareArticle(Articles article)
    {
        var articleToReturn = _mapper.Map<ArticleToReturnDto>(article);
        articleToReturn.TagList = await _tagRepository.GetTags(article.Id) as List<string>;
        articleToReturn.FavoritesCount = await _articleRepository.CountWhoFavoriteArticle(article.Id);
        var user = await _articleRepository.GetAuthor(article.Id);
        articleToReturn.Author = await _identity.PrepareProfile(HttpContext.User.Identity,user!.Username);
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

    [HttpGet("feed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ArticleToReturnDto>>> GetFeedArticles([FromQuery] ArticleResourceParameter? articleResourceParameter)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        if (!await _identity.IsExisted(username)) return NotFound();
        var articles = await _articleRepository.GetFeedArticles(articleResourceParameter,username);
        var articlesToReturn = await PrepareArticles(articles);
        return Ok(new {articles =articlesToReturn,articlesCount= articlesToReturn.Count()});
    }
}