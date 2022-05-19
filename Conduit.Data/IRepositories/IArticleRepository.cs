using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface IArticleRepository
{
    public Task<int> GetCurrentArticleCount();
    public Task CreateArticle(Articles createdArticle);
    public Task<int> Save();
    public Task CreateArticles(List<Articles> createdArticles);
}