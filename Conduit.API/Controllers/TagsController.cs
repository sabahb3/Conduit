using Conduit.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _tagRepository;

    public TagsController(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }
    /// <summary>
    /// Get all tags
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetTags()
    {
        var tags =await _tagRepository.GetTags();
        return Ok(new { Tags = tags });
    }
}