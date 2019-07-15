using CoreApi.Models;
using CoreApi.Models.BindingModels;
using SqlKata.Execution;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CoreApi.Services
{
    public class UserService
    {
        private readonly ITokenService _tokenService;
        private readonly QueryFactory _db;
        private ResponseModel response;

        public UserService(QueryFactory db, ITokenService tokenService)
        {
            this._db = db;
            this._tokenService = tokenService;
            this.response = new ResponseModel();
        }

        public ResponseModel Login(LoginBindingModel model)
        {
            User user = _db.Query("Users").Where("username", model.username).First<User>();

            if (user != null)
            {
                var usersClaims = new[]
                {
                    new Claim("ID", user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.username)
                };

                var generatedToken = _tokenService.GenerateAccessToken(usersClaims);
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(generatedToken);
                var refreshToken = _tokenService.GenerateRefreshToken();

                refreshTokenField(user.ID, refreshToken);
                response.SetResponseData(new
                {
                    token = jwtToken,
                    refreshToken = refreshToken,
                    expire = (long)(generatedToken.ValidTo - new DateTime(1970, 1, 1)).TotalMilliseconds
                });
            }
            else
            {
                response.SetError(1, "Fail Login");
            }

            return response;
        }

        public ResponseModel RefreshToken(RefreshTokenBindingModel model)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(model.token);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            User user = _db.Query("Users").Where("username", username).First<User>();
            if (user == null || user.refreshToken != model.refreshToken)
            {
                response.SetError(2, "Fail refresh token");
                return response;
            }

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateAccessToken(principal.Claims));
            var refreshToken = _tokenService.GenerateRefreshToken();

            refreshTokenField(user.ID, refreshToken);

            response.SetResponseData(new
            {
                token = jwtToken,
                refreshToken = refreshToken
            });

            return response;
        }

        public void Registration(RegistrationBindingModel model)
        {
            _db.Query("Users").Insert(
                new
                {
                    username = model.username,
                    password = model.password,
                    role = "user"
                });
        }

        private void refreshTokenField(int userID, string refreshToken)
        {
            _db.Query("Users").Where("ID", userID).Update(new
            {
                refreshToken = refreshToken
            });
        }
    }
}
