using Conduit.Domain;

namespace Conduit.Data.Models;

public class CommentToReturnDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Body { get; set; }
    public ProfileDto Author { get; set; }
}