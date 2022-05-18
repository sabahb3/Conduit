namespace Conduit.Domain;

public class Comments
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Body { get; set; }

    public Users User { get; set; }
    public string Username { get; set; }
    public Articles Article { get; set; }
    public int ArticlesId { get; set; }
}