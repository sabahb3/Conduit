﻿// <auto-generated />
using System;
using Conduit.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Conduit.Data.Migrations
{
    [DbContext(typeof(ConduitDbContext))]
    [Migration("20220518040658_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                });

            modelBuilder.Entity("Conduit.Domain.Followers", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FollowerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Username", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("Conduit.Domain.Tags", b =>
                {
                    b.Property<string>("Tag")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Tag");

                    b.ToTable("Tags");
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
                    b.HasOne("Conduit.Domain.Users", "Follower")
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Conduit.Domain.Users", "User")
                        .WithMany("Followers")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follower");

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
