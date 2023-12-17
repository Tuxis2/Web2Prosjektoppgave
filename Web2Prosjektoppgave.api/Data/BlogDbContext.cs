using Microsoft.EntityFrameworkCore;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Utilities;

namespace Web2Prosjektoppgave.api.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<BlogPostTag> BlogPostTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(user => user.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<User>()
            .Property(user => user.ModifiedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<Blog>()
            .Property(blog => blog.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<Blog>()
            .Property(blog => blog.ModifiedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<BlogPost>()
            .Property(post => post.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<BlogPost>()
            .Property(post => post.ModifiedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<Comment>()
            .Property(comment => comment.CreatedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<Comment>()
            .Property(comment => comment.ModifiedAt)
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        modelBuilder.Entity<BlogPost>()
            .HasOne(post => post.Blog)
            .WithMany()
            .HasForeignKey(post => post.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.BlogPost)
            .WithMany()
            .HasForeignKey(comment => comment.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Blog>()
            .HasOne(blog => blog.CreatedBy)
            .WithMany()
            .HasForeignKey(blog => blog.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Blog>()
            .HasOne(blog => blog.ModifiedBy)
            .WithMany()
            .HasForeignKey(blog => blog.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BlogPost>()
            .HasOne(post => post.CreatedBy)
            .WithMany()
            .HasForeignKey(post => post.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BlogPost>()
            .HasOne(post => post.ModifiedBy)
            .WithMany()
            .HasForeignKey(post => post.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BlogPost>()
            .HasMany(blogPost => blogPost.BlogPostTags)
            .WithMany(blogPostTags => blogPostTags.BlogPosts);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.CreatedBy)
            .WithMany()
            .HasForeignKey(comment => comment.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(comment => comment.ModifiedBy)
            .WithMany()
            .HasForeignKey(comment => comment.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);


        string salt1 = PasswordUtility.GenerateSaltBase64();
        string salt2 = PasswordUtility.GenerateSaltBase64();
        string hashedPassword1 = PasswordUtility.HashPassword("123", salt1);
        string hashedPassword2 = PasswordUtility.HashPassword("123", salt2);
        // Seeding
        modelBuilder.Entity<User>()
            .HasData(
                new User
                {
                    Id = 1,
                    CreatedById = 1,
                    ModifiedById = 1,
                    UserName = "BloggerDude1337",
                    Email = "bob@hotmail.com",
                    HashedPassword = hashedPassword1,
                    Salt = salt1
                },
                new User
                {
                    Id = 2,
                    CreatedById = 2,
                    ModifiedById = 2,
                    UserName = "Bloggo",
                    Email = "lisa@hotmail.com",
                    HashedPassword = hashedPassword2,
                    Salt = salt2
                }
            );

        modelBuilder.Entity<Blog>()
            .HasData(
                new Blog
                {
                    Id = 1,
                    CreatedById = 1,
                    ModifiedById = 1,
                    Title = "Blogger dudes bloggosphere",
                    Description = "I like my blogs"
                },
                new Blog
                {
                    Id = 2,
                    CreatedById = 1,
                    ModifiedById = 1,
                    Title = "Blogger dudes second sphere",
                    Description = "Real blogger dudes blog in plural"
                },
                new Blog
                {
                    Id = 3,
                    CreatedById = 2,
                    ModifiedById = 2,
                    Title = "Bloggo land",
                    Description = "My place for personal expression"
                }
            );

        modelBuilder.Entity<BlogPost>()
            .HasData(
                new BlogPost
                {
                    Id = 1,
                    CreatedById = 1,
                    ModifiedById = 1,
                    Title = "To blog, or not to blog.",
                    Content = "Is there purpose in blogging or is life without the stress of writing preferable..",
                    BlogId = 1,
                },
                new BlogPost
                {
                    Id = 2,
                    CreatedById = 2,
                    ModifiedById = 2,
                    Title = "My first blog post.",
                    Content = "I'm making my first blog to introduce myself to the community..",
                    BlogId = 3,
                }
            );

        modelBuilder.Entity<BlogPostTag>()
            .HasData(
                new BlogPostTag
                {
                    Id = 1,
                    Name = "lifestyle",
                },
                new BlogPostTag
                {
                    Id = 2,
                    Name = "outdoors",
                }
            );

        modelBuilder.Entity<Comment>()
            .HasData(
                new Comment
                {
                    Id = 1,
                    CreatedById = 2,
                    ModifiedById = 2,
                    Content = "I feel like there is purpose in blogging...",
                    BlogPostId = 1,
                },
                new Comment
                {
                    Id = 2,
                    CreatedById = 1,
                    ModifiedById = 1,
                    Content = "Hi, welcome to our little corned of the internet...",
                    BlogPostId = 2,
                }
            );
    }
}