using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Infrastructure.Enums;
using Kompanion.Api.Infrastructure.Exceptions;
using Kompanion.Api.Models.Entities;
using Kompanion.Api.Services;
using Kompanion.Api.Services.DBService;
using Kompanion.Api.Services.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kompanion.Api.Controllers
{
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IIdentityService _identityService;
        private readonly IDBService _dbService;

        public AccountController(ILogger<UserController> logger, IDBService dbService, IIdentityService identityService)
        {
            _logger = logger;
            _dbService = dbService;
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/login")]
        public async Task<IActionResult> Login([FromBody] AuthorizationDTO authorizationDTO)
        {
            var user = await _identityService.Authenticate(authorizationDTO.username, authorizationDTO.password);
            if(user == null)
            {
                throw new KompanionException(KompanionErrors.INVALID_USERNAME_OR_PASSWORD.Item1, KompanionErrors.INVALID_USERNAME_OR_PASSWORD.Item2);
            }

            return ResponseCreated(await _identityService.CreateUserClaims(user.UserId));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("account/register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if(string.IsNullOrEmpty(userDTO.Username) || string.IsNullOrEmpty(userDTO.Password) || string.IsNullOrEmpty(userDTO.PasswordConfirm))
            {
                throw new KompanionException(KompanionErrors.INVALID_USERNAME_OR_PASSWORD.Item1, KompanionErrors.INVALID_USERNAME_OR_PASSWORD.Item2);
            }

            if(userDTO.Password != userDTO.PasswordConfirm)
            {
                throw new KompanionException(KompanionErrors.PASSWORDS_DO_NOT_MATCH.Item1, KompanionErrors.PASSWORDS_DO_NOT_MATCH.Item2);
            }

            var user = new User
            {
                Username = userDTO.Username,
                CreateDate = DateTime.Now,
                UserType = (int)UserType.User,
                Status = (int)EntityStatus.Active
            };

            user = _identityService.CreatePasswordHashAndSalt(user, userDTO.Password);

            //TODO: var response = (User)await _dbService.Write(typeof(User), user);
            var response = (User)(await _dbService.ExecuteQuery(new Query
            {
                Procedure = UserProcedures.Create,
                Parameters = new Dictionary<string, object>
                {
                    { "Username", user.Username },
                    { "PasswordHash", user.PasswordHash },
                    { "PasswordSalt", user.PasswordSalt },
                    { "UserType", user.UserType },
                    { "Status", user.Status },
                    { "CreateDate", user.CreateDate }
                },
                ExpectedType = typeof(User)
            }))[0];

            return ResponseCreated(await _identityService.CreateUserClaims(response.UserId));
        }
    }
}
