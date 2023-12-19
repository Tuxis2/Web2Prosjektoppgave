using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web2Prosjektoppgave.api.Controllers;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.shared.ViewModels.Blog;
using Web2Prosjektoppgave.shared.ViewModels.BlogPost;

namespace Web2Prosjektoppgave.tests
{
    public class BlogPostControllerTest
    {
        [Fact]
        public async Task Index_Returns_OkObjectResult()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = 1
            };

            var testBlogs = new List<Blog>
            {
                new Blog() { Id = 1, CreatedBy = mockUser},
                new Blog() { Id = 2, CreatedBy = mockUser },
                new Blog() { Id = 3, CreatedBy = mockUser },
                new Blog() { Id = 4, CreatedBy = mockUser },
            };

            var mockBlogRepo = new Mock<IBlogRepository>();
            mockBlogRepo.Setup(repo => repo.GetAll())
                .ReturnsAsync(testBlogs);
            var mockBlogPostRepo = new Mock<IBlogPostRepository>();
            var mockCommentRepo = new Mock<ICommentRepository>();
            var mockAuthRepo = new Mock<IAuthorizationService>();

            var controller = new BlogController(
                mockBlogRepo.Object,
                mockBlogPostRepo.Object,
                mockCommentRepo.Object,
                mockAuthRepo.Object
            );

            // Act
            var result = await controller.Index();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = Assert.IsAssignableFrom<IEnumerable<BlogItemView>>(okResult.Value);
            Assert.Equal(4, viewModel.Count());
        }

        [Fact]
        public async Task GetMosRecent_Returns_OkObjectResult()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = 1
            };

            var testBlogs = new List<Blog>
            {
                new Blog() { Id = 1, CreatedBy = mockUser },
                new Blog() { Id = 2, CreatedBy = mockUser },
                new Blog() { Id = 3, CreatedBy = mockUser },
            };

            var mockBlogRepo = new Mock<IBlogRepository>();
            mockBlogRepo.Setup(repo => repo.GetMostRecent(3))
                .ReturnsAsync(testBlogs);
            var mockBlogPostRepo = new Mock<IBlogPostRepository>();
            var mockCommentRepo = new Mock<ICommentRepository>();
            var mockAuthRepo = new Mock<IAuthorizationService>();

            var controller = new BlogController(
                mockBlogRepo.Object,
                mockBlogPostRepo.Object,
                mockCommentRepo.Object,
                mockAuthRepo.Object
            );

            // Act
            var result = await controller.GetMostRecent();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = Assert.IsAssignableFrom<IEnumerable<BlogItemView>>(okResult.Value);
            Assert.Equal(3, viewModel.Count());
        }

        [Fact]
        public async Task Details_ReturnsOkObjectResult_When_ValidBlogPost()
        {
            // Arrange
            var mockBlogPostRepo = new Mock<IBlogPostRepository>();
            var mockUser = new User()
            {
                Id = 1
            };
            var existingBlogPost = new BlogPost()
            {
                Id = 1,
                CreatedBy = mockUser,
                ModifiedBy = mockUser,
            };
            mockBlogPostRepo.Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(existingBlogPost);

            var mockCommentRepo = new Mock<ICommentRepository>();
            mockCommentRepo.Setup(repo => repo.GetAllByBlogPostId(It.IsAny<int>()))
                .ReturnsAsync(new List<Comment>());

            var mockBlogRepo = new Mock<IBlogRepository>();
            var mockAuthRepo = new Mock<IAuthorizationService>();
            var mockBlogPostTagRepo = new Mock<IBlogPostTagRepository>();

            var controller = new BlogPostController(
                mockBlogPostRepo.Object,
                mockBlogPostTagRepo.Object,
                mockBlogRepo.Object,
                mockCommentRepo.Object,
                mockAuthRepo.Object
            );

            // Act
            var result = await controller.Details(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<BlogPostItemView>(okResult.Value);
        }

        [Fact]
        public async Task Create_Returns_CreatedResult_When_ValidBlog()
        {
            // Arrange
            var mockBlogRepo = new Mock<IBlogRepository>();
            mockBlogRepo.Setup(repo => repo.Insert(It.IsAny<Blog>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var mockBlogPostRepo = new Mock<IBlogPostRepository>();
            var mockCommentRepo = new Mock<ICommentRepository>();
            var mockAuthRepo = new Mock<IAuthorizationService>();

            var blogCreateForm = new BlogCreateForm()
            {
                Title = "New blog",
                Description = "This is a Blog about tea crumpets"
            };

            var controller = new BlogController(
                mockBlogRepo.Object,
                mockBlogPostRepo.Object,
                mockCommentRepo.Object,
                mockAuthRepo.Object
            );

            // Act
            var result = await controller.Create(blogCreateForm);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }
    }
}