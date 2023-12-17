using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.api.Utilities;
using Web2Prosjektoppgave.shared.ViewModels.Comment;
using Web2Prosjektoppgave.shared.ViewModels.User;

namespace Web2Prosjektoppgave.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAll();

            var viewModel = users.Select((user) => new UserItemView()
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                CreatedById = user.CreatedById,
                ModifiedAt = user.ModifiedAt,
                ModifiedById = user.ModifiedById,
                UserName = user.UserName,
                Email = user.Email,
            });

            return Ok(viewModel);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserItemView()
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                CreatedById = user.CreatedById,
                ModifiedAt = user.ModifiedAt,
                ModifiedById = user.ModifiedById,
                UserName = user.UserName,
                Email = user.Email,
            };

            return Ok(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateForm userForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(userForm);
            }

            var salt = PasswordUtility.GenerateSaltBase64();
            var hashedPassword = PasswordUtility.HashPassword(userForm.Password, salt);

            var postModel = new User()
            {
                UserName = userForm.UserName,
                Email = userForm.Email,
                HashedPassword = hashedPassword,
                Salt = salt,
            };

            await _userRepository.Insert(postModel);

            return Created();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit(UserEditForm userForm)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(userForm);
            }

            var existingUser = await _userRepository.GetById(1);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (ApiHelperFunctions.UpdatePropertiesDiffer(existingUser, userForm))
            {
                existingUser.UserName = userForm.UserName;
                existingUser.Email = userForm.Email;

                await _userRepository.Update(existingUser);
            }

            return Ok();
        }

        // Helper functions
        private bool UserPropertiesDiffer(User existingUser, UserEditForm userForm)
        {
            return existingUser.UserName != userForm.UserName || existingUser.Email != userForm.Email;
        }
    }

    
}
