using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Web2Prosjektoppgave.api.Controllers;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.shared.ViewModels.BlogPost;

namespace Web2Prosjektoppgave.tests
{
    public class BlogPostControllerTest
    {
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
            var model = Assert.IsType<BlogPostItemView>(okResult.Value);
        }
    }
}