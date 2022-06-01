using AutoMapper;
using Conduit.API.Helper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Microsoft.AspNetCore.Authentication;
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
    /// <summary>
    /// Favorite an article
    /// </summary>
    /// <param name="slug">Article's title to favorite</param>
    /// <returns>An article</returns>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">User not exist or the article not found</response>
    /// <response code="200">Article which you favorite </response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> FavoriteArticle(string slug)
    {
        var username = await _identity.GetLoggedUser(HttpContext.User.Identity,await HttpContext.GetTokenAsync("access_token"));
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
    /// <summary>
    /// Unfavorite an article
    /// </summary>
    /// <param name="slug">Article's title to unfavorite</param>
    /// <returns>An article</returns>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">User not exist or the article not found</response>
    /// <response code="200">Article which you unfavorite </response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> UnfavoriteArticle(string slug)
    {
        var username = await _identity.GetLoggedUser(HttpContext.User.Identity,await HttpContext.GetTokenAsync("access_token"));
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