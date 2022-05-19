using Conduit.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Conduit.Data;

public class ConduitDbContext : DbContext
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Articles> Articles { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbContextOptionsBuilder ConduitOptions { get; private set; }


    public ConduitDbContext()
    {
        
    }

    public ConduitDbContext(DbContextOptions options): base(options)
    {
        
    }
    public static readonly ILoggerFactory ConsoleLoggerFactory
        = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory).EnableSensitiveDataLogging()
                .UseSqlServer(
                    @"Server=localhost;
                    Database=Conduit;
                    Trusted_Connection=True;")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        ConduitOptions = optionsBuilder;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tags>().HasKey(t => t.Tag);
        modelBuilder.Entity<Users>().HasKey(u => u.Username);
        modelBuilder.Entity<UsersFavoriteArticles>().HasKey(u => new { u.Username, u.ArticleId });
        modelBuilder.Entity<Followers>().HasKey(u => new { u.Username, u.FollowingId });
        modelBuilder.Entity<ArticlesTags>().HasKey(a => new { a.ArticleId, a.Tag });
        
        modelBuilder.Entity<Comments>().HasOne(c => c.User)
            .WithMany(u => u.Comments).HasForeignKey(c => c.Username)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Comments>().HasOne(c => c.Article)
            .WithMany(a => a.Comments).HasForeignKey(c => c.ArticlesId);
        modelBuilder.Entity<Articles>().HasOne(a => a.User)
            .WithMany(u => u.Articles).HasForeignKey(a => a.Username);
        
        modelBuilder.Entity<UsersFavoriteArticles>().HasOne(u => u.User)
            .WithMany(u => u.UsersFavoriteArticles).HasForeignKey(u => u.Username)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UsersFavoriteArticles>().HasOne(u => u.Articles)
            .WithMany(a => a.UsersFavoriteArticles).HasForeignKey(u => u.ArticleId);
        modelBuilder.Entity<Followers>().HasOne(f => f.User)
            .WithMany(u => u.Followers).HasForeignKey(f => f.Username);
        modelBuilder.Entity<Followers>().HasOne(f => f.Following)
            .WithMany().HasForeignKey(f => f.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ArticlesTags>().HasOne(a => a.Articles)
            .WithMany(a => a.ArticlesTags).HasForeignKey(a => a.ArticleId);
        modelBuilder.Entity<ArticlesTags>().HasOne(a => a.Tags)
            .WithMany(t => t.ArticlesTags).HasForeignKey(a => a.Tag);
        
        modelBuilder.Entity<Users>().Property(u => u.ProfilePicture)
            .HasDefaultValue(@"https://api.realworld.io/images/smiley-cyrus.jpeg");
        modelBuilder.Entity<Users>().HasIndex(u => u.Email).IsUnique();
        SeedingUsers(modelBuilder);
        SeedingArticles(modelBuilder);
        SeedingComments(modelBuilder);
        SeedingTags(modelBuilder);
        SeedingFollowers(modelBuilder);
        SeedingFavoriteArticles(modelBuilder);
        SeedingArticlesTag(modelBuilder);
    }

    private void SeedingUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().HasData(
            new Users
            {
                Username = "sabah",
                Password = 4050.ToString(),
                Email = "sabahBaara4@gmail.com",
                ProfilePicture = @"https://api.realworld.io/images/smiley-cyrus.jpeg"
            },
            new Users
            {
                Username = "Shaymaa",
                Password = 1234.ToString(),
                Email = "shaymaaAshqar@gmail.com",
                ProfilePicture = @"https://api.realworld.io/images/smiley-cyrus.jpeg"

            },
            new Users
            {
                Username = "Hala",
                Password = 0000.ToString(),
                Email = "Hala@gmail.com",
                ProfilePicture = @"https://api.realworld.io/images/smiley-cyrus.jpeg"
            });
    }

    private void SeedingArticles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articles>().HasData(
            new Articles
            {
                Id = 1,
                Title = "Hello, and welcome",
                Description = "How to say hello",
                Body = @"We often start an English conversation with a simple “hello.”
                         You may see someone you know, make eye contact with a stranger, 
                         or start a phone conversation with this simple greeting. 
                         You may be asking yourself: “What should I say instead of “hello?",
                Date = new DateTime(2022,4,20),
                Username = "Hala"
                
            },
            new Articles
            {
                Id = 2,
                Title = "How are you",
                Description = "What do people really mean when they ask “How are you?",
                Body = @"When people start off an English conversation with “How are you?” 
                        they usually don’t expect you to go into much detail. 
                        Think of the “How are you?” question as a simple way to get the conversation going.",
                Date = new DateTime(2022,5,5),
                Username = "Hala"
            },
            new Articles
            {
                Id = 3,
                Title = "Be polite",
                Description = "Want to be polite? Be a mirror.",
                Body = @"In some situations it’s good to just repeat the same greeting 
                        back to your conversation partner.When you are meeting someone for the first time, 
                        it is considered polite to engage in this way.",
                Date = new DateTime(2022,2,22),
                Username = "Sabah"
            }
        );
    }

    private void SeedingComments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comments>().HasData(
            new Comments
            {
              Id =1,
              Date = new DateTime(2022,2,23),
              Body = "Keep going",
              Username = "Shaymaa",
              ArticlesId = 1
            },
            new Comments
            {
                Id =2,
                Date = new DateTime(2022,3,3),
                Body = "Awesome",
                Username = "Sabah",
                ArticlesId = 1
            },            
            new Comments
            {
                Id =3,
                Date = new DateTime(2022,5,5),
                Body = "Keep going",
                Username = "Shaymaa",
                ArticlesId = 2
            }
        );
    }

    private void SeedingTags(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tags>().HasData(
            new Tags
            {
                Tag = "Welcoming"
            },
            new Tags
            {
                Tag = "Conversation"
            },
            new Tags
            {
                Tag = "Greetings"
            }
            );
    }

    private void SeedingFollowers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Followers>().HasData(
            new Followers
            {
                Username = "Sabah",
                FollowingId = "Shaymaa"
            },
            new Followers
            {
                Username = "Sabah",
                FollowingId = "Hala"
            },
            new Followers
            {
                Username = "Shaymaa",
                FollowingId = "Sabah"
            }
            );
    }

    private void SeedingFavoriteArticles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsersFavoriteArticles>().HasData(
            new UsersFavoriteArticles
            {
                Username = "Sabah",
                ArticleId = 1
            },
            new UsersFavoriteArticles
            {
                Username = "Shaymaa",
                ArticleId = 1
            },
            new UsersFavoriteArticles
            {
                Username = "Hala",
                ArticleId = 3
            }
            );
    }

    private void SeedingArticlesTag(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArticlesTags>().HasData(
            new ArticlesTags
            {
                ArticleId = 1,
                Tag = "Welcoming"
            },
            new ArticlesTags
            {
                ArticleId = 1,
                Tag = "Greetings"
            },
            new ArticlesTags
            {
                ArticleId = 3,
                Tag = "Greetings"
            }
            );
    }
}