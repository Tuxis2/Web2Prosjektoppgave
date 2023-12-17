using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Web2Prosjektoppgave.api.Hubs;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.shared.ViewModels.BlogPost;
using Web2Prosjektoppgave.shared.ViewModels.Comment;

namespace Web2Prosjektoppgave.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHubContext<SignalHub, ISendComment> _hubContext;

        public CommentController(ICommentRepository repository, IBlogPostRepository blogPostRepository, IAuthorizationService authorizationService, IHubContext<SignalHub, ISendComment> hubContext)
        {
            _repository = repository;
            _blogPostRepository = blogPostRepository;
            _authorizationService = authorizationService;
            _hubContext = hubContext;
        }

        // Post
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateForm comment)
        {
            if (!ModelState.IsValid)
            {
                var blogPost = await _blogPostRepository.GetById(comment.BlogPostId);
                if (blogPost == null)
                {
                    return NotFound();
                }

                var blogPostView = new BlogPostItemView()
                {
                    Id = blogPost.BlogId,
                    CreatedAt = blogPost.CreatedAt,
                    CreatedByUserName = blogPost.CreatedBy.UserName,
                    ModifiedAt = blogPost.ModifiedAt,
                    ModifiedByUserName = blogPost.ModifiedBy.UserName,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                };

                var viewModel = new CommentCreateForm()
                {
                    BlogId = blogPost.BlogId,
                    BlogPostId = comment.BlogPostId,
                    BlogPost = blogPostView,
                };

                return BadRequest(viewModel);
            }

            var commentModel = new Comment()
            {
                //CreatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                //ModifiedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                CreatedById = 1,
                ModifiedById = 1,
                Content = comment.Content,
                BlogPostId = comment.BlogPostId,
            };

            //var requirement = new UserRequirement(commentModel.CreatedById);
            //// Check if the current user has the same ID as the one being requested
            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, requirement);

            //if (!authorizationResult.Succeeded)
            //{
            //    return new ForbidResult();
            //}

            await _repository.Insert(commentModel);

            commentModel = await _repository.GetById(commentModel.Id);

            await _hubContext.Clients.All.SendBlogPost("AddUserLater", new CommentItemSignal()
            {
                Id = commentModel.Id,
                BlogPostId = commentModel.BlogPostId,
                CreatedAt = commentModel.CreatedAt,
                CreatedByUserName = commentModel.CreatedBy.UserName,
                ModifiedAt = commentModel.ModifiedAt,
                ModifiedByUserName = commentModel.ModifiedBy.UserName,
                Content = commentModel.Content,
            });

            return Created();
        }

        // Put
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(CommentEditForm comment)
        {
            if (!ModelState.IsValid)
            {
                var blogPost = await _blogPostRepository.GetById(comment.BlogPostId);
                if (blogPost == null)
                {
                    return NotFound();
                }

                var blogPostView = new BlogPostItemView()
                {
                    Id = blogPost.BlogId,
                    CreatedAt = blogPost.CreatedAt,
                    CreatedByUserName = blogPost.CreatedBy.UserName,
                    ModifiedAt = blogPost.ModifiedAt,
                    ModifiedByUserName = blogPost.ModifiedBy.UserName,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                };

                var viewModel = new CommentEditForm()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    BlogId = blogPost.BlogId,
                    BlogPostId = comment.BlogPostId,
                    BlogPost = blogPostView
                };

                return BadRequest(viewModel);
            }

            var updateComment = await _repository.GetById(comment.Id);

            if (updateComment == null)
            {
                return NotFound();
            }

            //var requirement = new UserRequirement(updateComment.CreatedById);
            //// Check if the current user has the same ID as the one being requested
            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, requirement);

            //if (!authorizationResult.Succeeded)
            //{
            //    return new ForbidResult();
            //}

            if (updateComment.Content != comment.Content)
            {
                updateComment.ModifiedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                updateComment.ModifiedAt = DateTimeOffset.UtcNow;
                updateComment.Content = comment.Content;

                await _repository.Update(updateComment);
            }

            await _hubContext.Clients.All.SendBlogPost("Add user later", new CommentItemSignal()
            {
                Id = updateComment.Id,
                BlogPostId = updateComment.BlogPostId,
                //CreatedAt = updateComment.CreatedAt,
                //CreatedById = updateComment.CreatedById,
                //ModifiedAt = updateComment.ModifiedAt,
                //ModifiedById = updateComment.ModifiedById,
                Content = updateComment.Content,
            });

            return Ok();
        }

        // Delete
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int commentId)
        {
            var comment = await _repository.GetById(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            var blogPost = await _blogPostRepository.GetById(comment.BlogPostId);

            if (blogPost == null)
            {
                return NotFound();
            }

            //var requirement = new UserRequirement(comment.CreatedById);
            //// Check if the current user has the same ID as the one being requested
            //var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, requirement);

            //if (!authorizationResult.Succeeded)
            //{
            //    return new ForbidResult();
            //}

            await _repository.Delete(comment);

            return Ok();
        }
    }
}
