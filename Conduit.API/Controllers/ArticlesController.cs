using AutoMapper;
using Conduit.API.ResourceParameters;
using Conduit.Data.IRepositories;
using Conduit.Data.Models;
using Conduit.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
 private readonly IArticleRepository _articleRepository;
 private readonly IMapper _mapper;

 public ArticlesController(IArticleRepository articleRepository,IMapper mapper)
 {
  _articleRepository = articleRepository;
  _mapper = mapper;
 }
 [HttpGet]
 [AllowAnonymous]
 public async Task<IActionResult> GetArticles([FromQuery] ArticleResourceParameter? articleResourceParameter)
 {
  var articles = await _articleRepository.GetArticles(articleResourceParameter);
  return Ok(_mapper.Map<IEnumerable<ArticleToReturnDto>>(articles));
 }
}