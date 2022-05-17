namespace Conduit.Domain;

public class Articles
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Body { get; set; }
    public DateTime Date { get; set; }

    public string username { get; set; }
    public Users user { get; set; }
}