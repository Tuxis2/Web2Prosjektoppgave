using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Web2Prosjektoppgave.api.Models.Entities;
using Web2Prosjektoppgave.api.Models.Interfaces;
using Web2Prosjektoppgave.api.Utilities;
using Web2Prosjektoppgave.shared.ViewModels.Login;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Web2Prosjektoppgave.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Index(LoginView model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(model);
            }

            // Authenticate the user
            var authenticationResult = await AuthenticateUser(model.UserName, model.Password);
            if (authenticationResult.IsAuthenticated && authenticationResult.user != null)
            {
                var tokenString = CreateJWT(authenticationResult.user);

                return Ok(new { TokenString = tokenString });
            }

            return Unauthorized();
        }

        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    return RedirectToAction("Index", "Home");
        //}

        // Helper functions
        private async Task<(bool IsAuthenticated, User? user)> AuthenticateUser(string username, string password)
        {
            var user = await _userRepository.GetByUserNameOrEmail(username);

            if (user == null)
            {
                return (false, null);
            }

            return (PasswordUtility.VerifyPassword(password, user.HashedPassword, user.Salt), user);
        }

        private string CreateJWT(User user)

        {
            var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SECRET-KEY-MUST-BE-AT-LEAST-128-BITS-LONG"));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var securityToken = new JwtSecurityToken(
                issuer: "https://localhost:7238/",
                audience: "https://localhost:7238/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );

            var securityTokenHandler = new JwtSecurityTokenHandler();

            return securityTokenHandler.WriteToken(securityToken);
        }
    }
}
