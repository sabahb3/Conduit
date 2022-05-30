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
        var articleToReturn = await article.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity);
        return Ok(articleToReturn);
    }
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> UnfavoriteArticle(string slug)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        var isExist = await _identity.IsExisted(username);
        if (!isExist) return NotFound();
        var article=await _articleRepository.GetArticle(slug);
        if (article == null) return NotFound();
        await _articleRepository.UnfavoriteArticle(username, article.Id);
        await _articleRepository.Save();
        var articleToReturn =
            await article.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity);
        return Ok(articleToReturn);
    }
}