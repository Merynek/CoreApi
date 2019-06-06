using System.Security.Claims;
using CoreApi.Services;
using Microsoft.AspNetCore.Authorization;
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
                    new Claim("ID", "1"),
                    new Claim(ClaimTypes.Name, username),
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
                    new Claim("ID", "2"),
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

        /*[HttpPost]
        [AllowAnonymous]
        public async IActionResult RefreshToken(string authenticationToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(authenticationToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = _context.UserRefreshTokens.SingleOrDefault(u => u.Username == username);
            if (user == null || user.RefreshToken != refreshToken) return BadRequest();

            var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return new ObjectResult(new
            {
                authenticationToken = newJwtToken,
                refreshToken = newRefreshToken
            });
        }*/
    }
}
