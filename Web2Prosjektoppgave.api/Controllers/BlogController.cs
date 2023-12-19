using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.api.Repositories;
using Web2Prosjektoppgave.shared.ViewModels.Blog;
using Web2Prosjektoppgave.shared.ViewModels.BlogPost;
using Web2Prosjektoppgave.shared.ViewModels.Comment;

namespace Web2Prosjektoppgave.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthorizationService _authorizationService;

        public BlogController(IBlogRepository blogRepository, IBlogPostRepository blogPostRepository, ICommentRepository commentRepository, IAuthorizationService authorizationService)
        {
            _blogRepository = blogRepository;
            _blogPostRepository = blogPostRepository;
            _commentRepository = commentRepository;
            _authorizationService = authorizationService;
        }


        // Get
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogRepository.GetAll();

            var viewModel = blogs.Select((blog) => new BlogItemView()
            {
                Id = blog.Id,
                CreatedAt = blog.CreatedAt,
                CreatedByUserName = blog.CreatedBy.UserName,
                Title = blog.Title,
                Description = blog.Description,
            });

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("Recent")]
        public async Task<IActionResult> GetMostRecent()
        {
            var blogs = await _blogRepository.GetMostRecent(3);

            var viewModel = blogs.Select((blog) => new BlogItemView()
            {
                Id = blog.Id,
                CreatedAt = blog.CreatedAt,
                CreatedByUserName = blog.CreatedBy.UserName,
                Title = blog.Title,
                Description = blog.Description,
            });

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("{blogId}")]
        public async Task<IActionResult> Details(int blogId)
        {

            var blog = await _blogRepository.GetById(blogId);

            if (blog == null)
            {
                return NotFound();
            }

            var blogPosts = await _blogPostRepository.GetAllByBlogId(blog.Id);

            if (blogPosts == null)
            {
                return NotFound();
            }


            var blogPostItemViewList = new List<BlogPostItemView>();

            foreach (var post in blogPosts)
            {
                var postItem = new BlogPostItemView()
                {
                    Id = post.Id,
                    CreatedAt = post.CreatedAt,
                    CreatedByUserName = post.CreatedBy.UserName,
                    ModifiedAt = post.ModifiedAt,
                    ModifiedByUserName = post.ModifiedBy.UserName,
                    Title = post.Title,
                    Content = post.Content,
                };

                var commentRequest = await _commentRepository.GetAllByBlogPostId(post.Id);

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

                blogPostItemViewList.Add(postItem);
            }

            // var comments = await _commentRepository.GetAllByPostId()

            var viewModel = new BlogDetailView()
            {
                Id = blog.Id,
                CreatedAt = blog.CreatedAt,
                CreatedByUserName = blog.CreatedBy.UserName,
                ModifiedAt = blog.ModifiedAt,
                ModifiedByUserName = blog.ModifiedBy.UserName,
                Title = blog.Title,
                Description = blog.Description,
                BlogPosts = blogPostItemViewList,
            };

            return Ok(viewModel);
        }

        // Post
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateForm blog)
        {
            if (!ModelState.IsValid)
            {
                return Ok(blog);
            }

            var blogModel = new Blog()
            {
                CreatedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ModifiedById = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                Title = blog.Title,
                Description = blog.Description,
            };

            await _blogRepository.Insert(blogModel);

            return Created();
        }

        // Put
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit()
        {
            return Ok();
        }

        // Delete
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }
    }
}
