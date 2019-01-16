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

            var category = builder.Entity<Category>();
            category.HasKey(c => c.Id);
            category.Property(c => c.Name).IsRequired();


            var user = builder.Entity<User>();
            user.Property(u => u.RandomRegistrationCode).ValueGeneratedOnAdd();
            user.HasMany(u => u.News).WithOne(n => n.User).HasForeignKey(n => n.UserId);

            var news = builder.Entity<News>();
            news.HasKey(n => n.Id);
            news.Property(n => n.Content).IsRequired();
            news.Property(n => n.Description).IsRequired();
            news.Property(n => n.Title).IsRequired();

            var rating = builder.Entity<Rating>();
            rating.HasKey(r => r.Id);
            rating.Property(r => r.Value).IsRequired();
            rating.HasOne(r => r.News).WithMany(n => n.Ratings).HasForeignKey(r=>r.NewsId);
            rating.HasOne(r => r.User).WithMany(u => u.Ratings).HasForeignKey(r=>r.UserId);

            var comment = builder.Entity<Comment>();
            comment.HasKey(c => c.Id);
            comment.Property(c => c.Content).IsRequired();
            comment.HasOne(c => c.News).WithMany(n => n.Comments).HasForeignKey(c=>c.NewsId);
            comment.HasMany(c => c.Likes).WithOne(l => l.Comment).HasForeignKey(cl=>cl.CommentId);
            comment.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c=>c.UserId);

            var commentLike = builder.Entity<CommentLike>();
            commentLike.HasKey(cl => cl.Id);
            commentLike.HasOne(cl => cl.Comment).WithMany(c => c.Likes).HasForeignKey(cl=>cl.CommentId);
            commentLike.HasOne(cl => cl.User).WithMany(u => u.CommentLikes).HasForeignKey(cl=>cl.UserId);

            var language = builder.Entity<Language>();
            language.HasKey(l => l.Id);
            language.Property(l => l.Name).IsRequired();
            language.HasMany(l => l.Users).WithOne(u => u.Language).HasForeignKey(u=>u.LanguageId);

            var userProfile = builder.Entity<UserProfile>();
            userProfile.HasKey(u => u.Id);
            userProfile.Property(u => u.FirstName).IsRequired();
            userProfile.Property(u => u.LastName).IsRequired();
            userProfile.HasOne(up => up.User).WithOne(u => u.UserProfile).HasForeignKey<User>(u => u.UserProfileId);

        }

       
    }
}
