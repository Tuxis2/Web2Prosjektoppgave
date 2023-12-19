using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.api.Security;
using Web2Prosjektoppgave.shared.ViewModels.BlogPost;
using Web2Prosjektoppgave.shared.ViewModels.Comment;

namespace Web2Prosjektoppgave.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostTagRepository _blogPostTagRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorizationService _authorizationService;

        public BlogPostController(
            IBlogPostRepository blogPostRepository,
            IBlogPostTagRepository blogTagRepository,
            IBlogRepository blogRepository,
            ICommentRepository commentRepository,
            IAuthorizationService authorizationService)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostTagRepository = blogTagRepository;
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _authorizationService = authorizationService;
        }

        // Get
        [HttpGet]
        [Route("{blogPostId}")]
        public async Task<IActionResult> Details(int blogPostId)
        {

            var blogPost = await _blogPostRepository.GetById(blogPostId);

            if (blogPost == null)
            {
                return NotFound();
            }

            
            var postItem = new BlogPostItemView()
            {
                Id = blogPost.Id,
                CreatedAt = blogPost.CreatedAt,
                CreatedByUserName = blogPost.CreatedBy.UserName,
                ModifiedAt = blogPost.ModifiedAt,
                ModifiedByUserName = blogPost.ModifiedBy.UserName,
                Title = blogPost.Title,
                Content = blogPost.Content,
            };

            var commentRequest = await _commentRepository.GetAllByBlogPostId(blogPost.Id);

            if (commentRequest != null)
            {
                postItem.BlogComments = commentRequest.Select(comment => new CommentItemView()
                {
                    Id = comment.Id,
                    CreatedAt = comment.CreatedAt,
                    CreatedByUserName = comment.CreatedBy.UserName,
                    ModifiedAt = comment.ModifiedAt,
                    ModifiedByUserName = comment.ModifiedBy.UserName,
                    Content = comment.Content,
                }).ToList();
            }

            return Ok(postItem);
        }

        // Post
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogPostCreateForm blogPostForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(blogPostForm);
            }

            var blog = await _blogRepository.GetById(blogPostForm.BlogId);

            if (blog == null)
            {
                return NotFound();
            }

            var requirement = new UserRequirement(blog.CreatedById);
            // Check if the current user has the same ID as the one being requested
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, requirement);

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var tagList = new List<BlogPostTag>();
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(blogPostForm.Content);
            foreach (var match in matches)
            {
                var exists = await _blogPostTagRepository.GetByName(match.ToString());
                if (exists == null)
                {
                    tagList.Add(new BlogPostTag
                    {
                        Name = match.ToString(),
                    });
                }
                else
                {
                    tagList.Add(exists);
                }
            }

            var postModel = new BlogPost()
            {
                CreatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Title = blogPostForm.Title,
                Content = blogPostForm.Content,
                BlogId = blogPostForm.BlogId,
                BlogPostTags = tagList
            };

            await _blogPostRepository.Insert(postModel);

            return Created();
        }

        // Put
        //[Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(BlogPostEditForm blogPostForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(blogPostForm);
            }

            var updatePost = await _blogPostRepository.GetById(blogPostForm.Id);

            if (updatePost == null)
            {
                return NotFound();
            }

            var requirement = new UserRequirement(updatePost.CreatedById);
            // Check if the current user has the same ID as the one being requested
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, requirement);

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            var tagList = new List<BlogPostTag>();
            var regex = new Regex(@"#\w+");
            var matches = regex.Matches(blogPostForm.Content);
            foreach (var match in matches)
            {
                var exists = await _blogPostTagRepository.GetByName(match.ToString());
                if (exists == null)
                {
                    tagList.Add(new BlogPostTag
                    {
                        Name = match.ToString(),
                    });
                }
                else
                {
                    tagList.Add(exists);
                }
            }

            if (updatePost.Title != blogPostForm.Title || updatePost.Content != blogPostForm.Content)
            {
                updatePost.ModifiedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                updatePost.ModifiedAt = DateTimeOffset.UtcNow;
                updatePost.Title = blogPostForm.Title;
                updatePost.Content = blogPostForm.Content;
                updatePost.BlogPostTags = tagList;

                await _blogPostRepository.Update(updatePost);
            }

            return Ok();
        }

        // Delete
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int blogPostId)
        {
            var blogPost = await _blogPostRepository.GetById(blogPostId);

            if (blogPost == null)
            {
                return NotFound();
            }

            var requirement = new UserRequirement(blogPost.CreatedById);
            // Check if the current user has the same ID as the one being requested
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, requirement);

            if (!authorizationResult.Succeeded)
            {
                return new ForbidResult();
            }

            await _blogPostRepository.Delete(blogPost);

            return Ok();
        }

        //[Authorize]
        [HttpGet]
        [Route("Search/{blogId}")]
        public async Task<IActionResult> Search(string phrase, int blogId)
        {
            var blogPosts = await _blogPostRepository.Search(phrase, blogId);
            var listOfBlogPosts = new List<BlogPostItemView>();

            foreach (var blogPost in blogPosts)
            {
                var commentRequest = await _commentRepository.GetAllByBlogPostId(blogPost.Id);

                listOfBlogPosts.Add(new BlogPostItemView()
                {
                    Id = blogPost.Id,
                    CreatedAt = blogPost.CreatedAt,
                    CreatedByUserName = blogPost.CreatedBy.UserName,
                    ModifiedAt = blogPost.ModifiedAt,
                    ModifiedByUserName = blogPost.ModifiedBy.UserName,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    BlogComments = commentRequest.Select(comment => new CommentItemView()
                    {
                        Id = comment.Id,
                        CreatedAt = comment.CreatedAt,
                        CreatedByUserName = comment.CreatedBy.UserName,
                        ModifiedAt = comment.ModifiedAt,
                        ModifiedByUserName = comment.ModifiedBy.UserName,
                        Content = comment.Content,
                    }).ToList(),
            });
            }

            return Ok(listOfBlogPosts);
        }
    }
}
