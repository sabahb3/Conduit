namespace Conduit.Data.Models;

public class ProfileDto
{
    public string Username { get; set; }
    public string Bio { get; set; }
    public string image { get; set; }= String.Empty;
    public bool Following { get; set; }
}