﻿// <auto-generated />
using System;
using Conduit.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Conduit.Data.Migrations
{
    [DbContext(typeof(ConduitDbContext))]
    partial class ConduitDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Conduit.Domain.Articles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Body = "We often start an English conversation with a simple “hello.”\r\n                         You may see someone you know, make eye contact with a stranger, \r\n                         or start a phone conversation with this simple greeting. \r\n                         You may be asking yourself: “What should I say instead of “hello?",
                            Date = new DateTime(2022, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "How to say hello",
                            Title = "Hello, and welcome",
                            Username = "Hala"
                        },
                        new
                        {
                            Id = 2,
                            Body = "When people start off an English conversation with “How are you?” \r\n                        they usually don’t expect you to go into much detail. \r\n                        Think of the “How are you?” question as a simple way to get the conversation going.",
                            Date = new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "What do people really mean when they ask “How are you?",
                            Title = "How are you",
                            Username = "Hala"
                        },
                        new
                        {
                            Id = 3,
                            Body = "In some situations it’s good to just repeat the same greeting \r\n                        back to your conversation partner.When you are meeting someone for the first time, \r\n                        it is considered polite to engage in this way.",
                            Date = new DateTime(2022, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Want to be polite? Be a mirror.",
                            Title = "Be polite",
                            Username = "Sabah"
                        });
                });

            modelBuilder.Entity("Conduit.Domain.ArticlesTags", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ArticleId", "Tag");

                    b.HasIndex("Tag");

                    b.ToTable("ArticlesTags");
                });

            modelBuilder.Entity("Conduit.Domain.Comments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ArticlesId")
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticlesId");

                    b.HasIndex("Username");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArticlesId = 1,
                            Body = "Keep going",
                            Date = new DateTime(2022, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "Shaymaa"
                        },
                        new
                        {
                            Id = 2,
                            ArticlesId = 1,
                            Body = "Awesome",
                            Date = new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "Sabah"
                        },
                        new
                        {
                            Id = 3,
                            ArticlesId = 2,
                            Body = "Keep going",
                            Date = new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Username = "Shaymaa"
                        });
                });

            modelBuilder.Entity("Conduit.Domain.Followers", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FollowingId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Username", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("Followers");

                    b.HasData(
                        new
                        {
                            Username = "Sabah",
                            FollowingId = "Shaymaa"
                        },
                        new
                        {
                            Username = "Sabah",
                            FollowingId = "Hala"
                        },
                        new
                        {
                            Username = "Shaymaa",
                            FollowingId = "Sabah"
                        });
                });

            modelBuilder.Entity("Conduit.Domain.Tags", b =>
                {
                    b.Property<string>("Tag")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Tag");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Tag = "Welcoming"
                        },
                        new
                        {
                            Tag = "Conversation"
                        },
                        new
                        {
                            Tag = "Greetings"
                        });
                });

            modelBuilder.Entity("Conduit.Domain.Users", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("https://api.realworld.io/images/smiley-cyrus.jpeg");

                    b.HasKey("Username");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Username = "sabah",
                            Email = "sabahBaara4@gmail.com",
                            Password = "4050"
                        },
                        new
                        {
                            Username = "Shaymaa",
                            Email = "shaymaaAshqar@gmail.com",
                            Password = "1234"
                        },
                        new
                        {
                            Username = "Hala",
                            Email = "Hala@gmail.com",
                            Password = "0"
                        });
                });

            modelBuilder.Entity("Conduit.Domain.UsersFavoriteArticles", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.HasKey("Username", "ArticleId");

                    b.HasIndex("ArticleId");

                    b.ToTable("UsersFavoriteArticles");

                    b.HasData(
                        new
                        {
                            Username = "Sabah",
                            ArticleId = 1
                        },
                        new
                        {
                            Username = "Shaymaa",
                            ArticleId = 1
                        },
                        new
                        {
                            Username = "Hala",
                            ArticleId = 3
                        });
                });

            modelBuilder.Entity("Conduit.Domain.Articles", b =>
                {
                    b.HasOne("Conduit.Domain.Users", "User")
                        .WithMany("Articles")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Conduit.Domain.ArticlesTags", b =>
                {
                    b.HasOne("Conduit.Domain.Articles", "Articles")
                        .WithMany("ArticlesTags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Conduit.Domain.Tags", "Tags")
                        .WithMany("ArticlesTags")
                        .HasForeignKey("Tag")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articles");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Conduit.Domain.Comments", b =>
                {
                    b.HasOne("Conduit.Domain.Articles", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticlesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Conduit.Domain.Users", "User")
                        .WithMany("Comments")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Conduit.Domain.Followers", b =>
                {
                    b.HasOne("Conduit.Domain.Users", "Following")
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Conduit.Domain.Users", "User")
                        .WithMany("Followers")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Following");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Conduit.Domain.UsersFavoriteArticles", b =>
                {
                    b.HasOne("Conduit.Domain.Articles", "Articles")
                        .WithMany("UsersFavoriteArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Conduit.Domain.Users", "User")
                        .WithMany("UsersFavoriteArticles")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Articles");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Conduit.Domain.Articles", b =>
                {
                    b.Navigation("ArticlesTags");

                    b.Navigation("Comments");

                    b.Navigation("UsersFavoriteArticles");
                });

            modelBuilder.Entity("Conduit.Domain.Tags", b =>
                {
                    b.Navigation("ArticlesTags");
                });

            modelBuilder.Entity("Conduit.Domain.Users", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Comments");

                    b.Navigation("Followers");

                    b.Navigation("UsersFavoriteArticles");
                });
#pragma warning restore 612, 618
        }
    }
}
