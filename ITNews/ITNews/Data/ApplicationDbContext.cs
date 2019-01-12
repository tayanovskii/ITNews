using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ITNews.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var tag = builder.Entity<Tag>();
            tag.HasKey(t => t.Id);
            tag.Property(t => t.Name).IsRequired();
            tag.HasMany(t => t.NewsTags)
                .WithOne(nt => nt.Tag)
                .HasForeignKey(t => t.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            var newsTag = builder.Entity<NewsTag>();
            newsTag.HasKey(nt => new {nt.NewsId, nt.TagId});
            newsTag.HasOne(t => t.Tag).WithMany(nt => nt.NewsTags).HasForeignKey(t => t.TagId);
            newsTag.HasOne(n => n.News).WithMany(nt => nt.NewsTags).HasForeignKey(n => n.NewsId);

            var newsCategory = builder.Entity<NewsCategory>();
            newsCategory.HasKey(nc => new {nc.NewsId, nc.CategoryId});
            newsCategory.HasOne(ns => ns.News).WithMany(n => n.NewsCategories).HasForeignKey(ns => ns.NewsId);
            newsCategory.HasOne(ns => ns.Category).WithMany(c => c.NewsCategories).HasForeignKey(ns => ns.CategoryId);

            var user = builder.Entity<User>();
            user.Property(u => u.RandomRegistrationCode).ValueGeneratedOnAdd();
        }

       
    }
}
