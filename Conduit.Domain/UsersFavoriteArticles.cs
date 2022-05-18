namespace Conduit.Domain;

public class UsersFavoriteArticles
{
    public string Username { get; set; }
    public int ArticleId { get; set; }

    public Users User { get; set; }
    public Articles Articles { get; set; }
}