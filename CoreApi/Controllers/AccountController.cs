using System.Security.Claims;
using CoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITokenService _tokenService;

        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin")
            {
                var usersClaims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "admin")

                };

                var jwtToken = _tokenService.GenerateAccessToken(usersClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                /*user.RefreshToken = refreshToken;
                await _usersDb.SaveChangesAsync();*/

                return Ok(new
                {
                    token = jwtToken,
                    refreshToken = refreshToken
                });
            }

            if (username == "merynek")
            {
                var usersClaims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                };

                var jwtToken = _tokenService.GenerateAccessToken(usersClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                /*user.RefreshToken = refreshToken;
                await _usersDb.SaveChangesAsync();*/

                return Ok(new
                {
                    token = jwtToken,
                    refreshToken = refreshToken
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
