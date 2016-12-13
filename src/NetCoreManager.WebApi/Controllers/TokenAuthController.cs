using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetCoreManager.Application.Interface;
using NetCoreManager.Component.Tools.Service;
using NetCoreManager.Component.Tools.Encrypt;
using NetCoreManager.Domain.Entity;
using NetCoreManager.WebApi.Auth;
using NetCoreManager.WebApi.Model;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreManager.WebApi.Controllers
{
    [Route("tokenAuth")]
    public class TokenAuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationConfigurationService _applicationConfigurationService;

        public TokenAuthController(IUserService userService, ApplicationConfigurationService applicationConfigurationService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (applicationConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(applicationConfigurationService));
            }
            _userService = userService;
            _applicationConfigurationService = applicationConfigurationService;
        }

        [Route("getAuthToken")]
        public async Task<string> GetAuthToken(LoginModel user)
        {
            user.Password = EncryptHelper.Encrypt(user.Password, _applicationConfigurationService.AppConfigurations.PwdSalt);
            var existUser =await _userService.Login(user.Account, user.Password);
            if (existUser != null)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

                return JsonConvert.SerializeObject(new
                {
                    stateCode = 1,
                    requertAt = requestAt,
                    expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                    accessToken = token
                });
            }
            return "";
        }

        private string GenerateToken(User user, DateTime expries)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Account, "TokenAuth"),
                new[] { new Claim("ID", user.Id.ToString()), }
                );
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expries
            });
            return handler.WriteToken(securityToken);
        }
    }
}
