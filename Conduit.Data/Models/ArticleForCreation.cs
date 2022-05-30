using Conduit.Domain;

namespace Conduit.Data.Models;

public class ArticleForCreation
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}