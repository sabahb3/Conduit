using AutoMapper;
using Conduit.API.Helper;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Domain;
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddNewComment(string slug, CommentForCreationDto createdComment)
    {
        var username = _userIdentity.GetLoggedUser(HttpContext.User.Identity);
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