namespace Conduit.Domain;

public class ArticlesTags
{
    public int ArticleId { get; set; }
    public string Tag { get; set; }

    public Articles Articles { get; set; }
    public Tags Tags { get; set; }
}