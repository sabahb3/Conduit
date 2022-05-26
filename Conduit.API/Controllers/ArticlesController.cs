using System.Security.Claims;
using AutoMapper;
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
    private readonly IUserRepository _userRepository;

    public ArticlesController(IArticleRepository articleRepository, IMapper mapper, ITagRepository tagRepository,
        IUserRepository userRepository)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
        _tagRepository = tagRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetArticles([FromQuery] ArticleResourceParameter? articleResourceParameter)
    {
        var articles = await _articleRepository.GetArticles(articleResourceParameter);
        var articlesToReturn = await PrepareArticles(articles);
        return Ok(articlesToReturn);
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
        articleToReturn.Author = await PrepareProfile(user!.Username);
        articleToReturn.Favorited = await IsFavorited(article.Id);
        return articleToReturn;
    }

    [NonAction]
    public async Task<bool> IsFavorited(int articleId)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity == null) return false;
        var username = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
        return await _articleRepository.DoesFavoriteArticle(username!, articleId);
    }

    private async Task<ProfileDto?> PrepareProfile(string username)
    {
        var userEntity = await _userRepository.GetUser(username);
        if (userEntity == null)
            return null;
        var profile = _mapper.Map<ProfileDto>(userEntity);
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userIdentity = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            profile.Following = await _userRepository.DoesFollow(userIdentity!, username);
        }

        return profile;
    }
}