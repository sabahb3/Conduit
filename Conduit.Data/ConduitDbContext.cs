using Conduit.Domain;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Data;

public class ConduitDbContext : DbContext
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Articles> Articles { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Tags> Tags { get; set; }
}