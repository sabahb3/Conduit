using AutoMapper;
using Conduit.API.Helper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/articles/{slug}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly UserIdentity _userIdentity;
    private readonly IMapper _mapper;

    public CommentsController(ICommentRepository commentRepository, UserIdentity userIdentity,IMapper mapper)
    {
        _commentRepository = commentRepository;
        _userIdentity = userIdentity;
        _mapper = mapper;
    }
    /// <summary>
    /// Add a new comment
    /// </summary>
    /// <param name="slug">Article's title you want to add a comment to </param>
    /// <param name="createdComment">Comment's body</param>
    /// <returns>Added comment</returns>
    /// <response code="401">Unauthorized user</response>
    /// <response code="404">No user with this username, or no article with this title</response>
    /// <response code="422">Invalid state of the new comment</response>
    /// <response code="200">Added comment</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CommentToReturnDto>> AddNewComment(string slug, CommentForCreationDto createdComment)
    {
        var username = await _userIdentity.GetLoggedUser(HttpContext.User.Identity,await HttpContext.GetTokenAsync("access_token"));
        if (username == null) return Unauthorized();
        var user = await _userIdentity.IsExisted(username);
        if (!user) return NotFound();
        var articleId = await _commentRepository.GetArticleId(slug);
        if (articleId==null) return NotFound();
        if (!TryValidateModel(createdComment))
        {
            return ValidationProblem(ModelState);
        }

        var commentEntity = _mapper.Map<Comments>(createdComment);
        commentEntity.Username = username;
        commentEntity.ArticlesId = articleId.Value;
        await _commentRepository.CreateComment(commentEntity);
        await _commentRepository.Save();
        var commentToReturn =await PrepareComment(commentEntity);
        return Ok(new {Comment=commentToReturn});
    }
    /// <summary>
    /// Get article's comments
    /// </summary>
    /// <param name="slug">Asked article</param>
    /// <returns>Article's comments</returns>
    /// <response code="404">No article with this title</response>
    /// <response code="200">Asked comments</response>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CommentToReturnDto>>> GetArticleComments(string slug)
    {
        var articleId = await _commentRepository.GetArticleId(slug);
        if (articleId==null) return NotFound();
        var comments = await _commentRepository.ReadArticleComments(slug);
        var commentsToReturn = await PrepareComments(comments!);
        return Ok(new { Comments = commentsToReturn });
    }

    /// <summary>
    /// Delete a specific comment
    /// </summary>
    /// <param name="slug">article title</param>
    /// <param name="id">Comment id</param>
    /// <returns></returns>
    /// <response code="404">No user with this username, no article with this title, or no comment with this id</response>
    /// <response code="204">Comment removed</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Try to remove a  comment that does not belong to you</response>

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> RemoveComment(string slug,int id)
    {
        var username = await _userIdentity.GetLoggedUser(HttpContext.User.Identity,await HttpContext.GetTokenAsync("access_token"));
        if (username == null) return Unauthorized();
        var isExist = await _userIdentity.IsExisted(username);
        if (!isExist) return NotFound();
        var article = await _commentRepository.GetArticleId(slug);
        if (article == null) return NotFound();
        var comment = await _commentRepository.GetComment(id);
        if (comment == null) return NotFound();
        if (comment.Username != username) return Forbid();
        var validComment = await _commentRepository.DoesArticleHasComment(article.Value, id);
        if (!validComment) return NotFound();
        await _commentRepository.DeleteComment(id);
        await _commentRepository.Save();
        return NoContent();
    }

    [NonAction]
    private async Task<CommentToReturnDto> PrepareComment(Comments comment)
    {
        var commentToReturn = _mapper.Map<CommentToReturnDto>(comment);
        commentToReturn.Author = await _userIdentity.PrepareProfile(HttpContext.User.Identity, comment.Username);
        return commentToReturn;
    }
    [NonAction]
    private async Task<IEnumerable<CommentToReturnDto>> PrepareComments(IEnumerable<Comments> comments)
    {
        var commentsToReturn = new List<CommentToReturnDto>();
        foreach (var comment in comments) commentsToReturn.Add(await PrepareComment(comment));
        return commentsToReturn;
    }
    public override ActionResult ValidationProblem([ActionResultObjectValue]ModelStateDictionary modelStateDictionary)
    {
        Console.WriteLine($"{modelStateDictionary.ValidationState} {modelStateDictionary.IsValid}");
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
        return (ActionResult) options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }

}