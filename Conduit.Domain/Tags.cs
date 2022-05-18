namespace Conduit.Domain;

public class Tags
{
    public Tags()
    {
        ArticlesTags = new List<ArticlesTags>();
    }
    public string Tag { get; set; }
    public ICollection<ArticlesTags> ArticlesTags { get; set; }
}