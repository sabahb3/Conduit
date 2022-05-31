using AutoMapper;
using Conduit.API.Helper;
using Conduit.API.ResourceParameters;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Conduit.API;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;
    private readonly UserIdentity _identity;

    public ArticlesController(IArticleRepository articleRepository, IMapper mapper, UserIdentity identity)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
        _identity = identity;
    }

    /// <summary>
    /// Get Conduit' articles 
    /// </summary>
    /// <param name="articleResourceParameter">Filtering articles based on tag, author, favorited by user, and asked page</param>
    /// <returns>Asked articles</returns>
    /// <response code="200">Returns list of articles</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ArticleToReturnDto>>> GetArticles([FromQuery] ArticleResourceParameter? articleResourceParameter)
    {
        var articles = await _articleRepository.GetArticles(_articleRepository.Articles, articleResourceParameter);
        var articlesToReturn = await PrepareArticles(articles);
        return Ok(new {articles =articlesToReturn,articlesCount= articlesToReturn.Count()});
    }

    [NonAction]
    public async Task<IEnumerable<ArticleToReturnDto>> PrepareArticles(IEnumerable<Articles> articles)
    {
        var articlesToReturn = new List<ArticleToReturnDto>();
        foreach (var article in articles) 
            articlesToReturn.Add(await article.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity));
        return articlesToReturn;
    }
    
    /// <summary>
    /// Get articles written by users you follow 
    /// </summary>
    /// <param name="articleResourceParameter">Filtering articles based on tag, author, favorited by user, and asked page</param>
    /// <returns>Asked articles</returns>
    /// <response code="200">Returns list of articles</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">When the token is valid but the user does not exist anymore</response>
    [HttpGet("feed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ArticleToReturnDto>>> GetFeedArticles([FromQuery] ArticleResourceParameter? articleResourceParameter)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        if (!await _identity.IsExisted(username)) return NotFound();
        var articles = await _articleRepository.GetFeedArticles(articleResourceParameter,username);
        var articlesToReturn = await PrepareArticles(articles);
        return Ok(new {articles =articlesToReturn,articlesCount= articlesToReturn.Count()});
    }

    /// <summary>
    /// Get an article based on its slug
    /// </summary>
    /// <param name="slug">article's slug</param>
    /// <returns>An article</returns>
    /// <response code="404">When there is no article with this title</response>
    /// <response code="200">Get asked article</response>
    [AllowAnonymous]
    [HttpGet("{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleToReturnDto>> GetArticle(string slug)
    {
        var article = await _articleRepository.GetArticleBySlug(slug);
        if (article == null) return NotFound();
        var articleToReturn = await article.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity);
        return Ok(new { article = articleToReturn });
    }

    /// <summary>
    /// Create new article
    /// </summary>
    /// <param name="createdArticle">Add a new article by giving a title, description, and body. You may add tags</param>
    /// <returns>Created article</returns>
    /// <response code="200">Get asked article</response>

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> CreateArticle(ArticleForCreation createdArticle)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        if (!await _identity.IsExisted(username)) return NotFound();
        if (!TryValidateModel(createdArticle)) return ValidationProblem(ModelState);
        var articleEntity = _mapper.Map<Articles>(createdArticle);
        articleEntity.Username = username;
        await _articleRepository.CreateArticle(articleEntity);
        await _articleRepository.Save();
        var articleToReturn =await articleEntity.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity);
        return Ok(articleToReturn);
    }

    /// <summary>
    /// Update an article
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="updatedArticle"></param>
    /// <returns>updated article</returns>
    /// <response code="200">Update an article</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">There is no user with this username, or no articles with this slug</response>
    /// <response code="422">Invalid state for updated article</response>
    /// <response code="403">Try to update articles that do not belong to you</response>
    [HttpPut("{slug}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> UpdateArticle(string slug, ArticleForUpdatingDto updatedArticle)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        var user = await _identity.IsExisted(username);
        if (!user) return NotFound();
        var article = await _articleRepository.GetArticle(slug);
        if (article == null) return NotFound();
        if (article.Username != username) return Forbid();
        if (!TryValidateModel(updatedArticle)) return ValidationProblem(ModelState);
        _mapper.Map(updatedArticle, article);
        await _articleRepository.UpdateArticle(article);
        await _articleRepository.Save();
        var articleToReturn =await article.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity);
        return Ok(articleToReturn);
    }
    /// <summary>
    /// Update an article
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="patchDocument">The set of operations to apply on the article</param>
    /// <returns>updated article</returns>
    /// <remarks>
    ///     Sample Request : This will update article's title
    ///     ```
    ///     [
    ///     {
    ///     "op": "replace",
    ///     "path": "/title",
    ///     "value": "Updated title"
    ///     }
    ///     ]
    ///     ```
    /// </remarks>
    /// <response code="200">Update an article</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">There is no user with this username, or no articles with this slug</response>
    /// <response code="422">Invalid state for updated article</response>
    /// <response code="403">Try to update articles that do not belong to you</response>
    [HttpPatch("{slug}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleToReturnDto>> PartialUpdate(string slug,
        JsonPatchDocument<ArticleForUpdatingDto> patchDocument)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        var user = await _identity.IsExisted(username);
        if (!user) return NotFound();
        var article = await _articleRepository.GetArticle(slug);
        if (article == null) return NotFound();
        if (article.Username != username) return Forbid();
        var articleToUpdate = _mapper.Map<ArticleForUpdatingDto>(article);
        patchDocument.ApplyTo(articleToUpdate);
        if (!TryValidateModel((articleToUpdate)))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(articleToUpdate, article);
        await _articleRepository.UpdateArticle(article);
        await _articleRepository.Save();
        var articleToReturn =await article.PrepareArticle(_mapper, _articleRepository, _identity, HttpContext.User.Identity);
        return Ok(articleToReturn);
    }
    /// <summary>
    /// Delete an article
    /// </summary>
    /// <param name="slug"></param>
    /// <returns>updated article</returns>
    /// <response code="204">The article deleted</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">There is no user with this username, or no articles with this slug</response>
    /// <response code="403">Try to update articles that does not belong to you</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("{slug}")]
    public async Task<ActionResult> DeleteArticle(string slug)
    {
        var username = _identity.GetLoggedUser(HttpContext.User.Identity);
        if (username == null) return Unauthorized();
        var user = await _identity.IsExisted(username);
        if (!user) return NotFound();
        var article = await _articleRepository.GetArticle(slug);
        if (article == null) return NotFound();
        if (article.Username != username) return Forbid();
        await _articleRepository.RemoveArticle(slug,username);
        await _articleRepository.Save();
        return NoContent();
    }

    public override ActionResult ValidationProblem([ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
    {
        Console.WriteLine($"{modelStateDictionary.ValidationState} {modelStateDictionary.IsValid}");
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
        return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}