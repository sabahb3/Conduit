using Conduit.Domain;

namespace Conduit.Data.IRepositories;

public interface ITagRepository
{
    public Task<IEnumerable<Tags>> GetTags(int articleId);
    public Task<int> Save();
    public Task AddTags(int articleId, List<Tags> tags);
    public Task<List<string>> GetPopularTag();

}