using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kompanion.Api.Helpers;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Infrastructure.Enums;
using Kompanion.Api.Infrastructure.Exceptions;
using Kompanion.Api.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Kompanion.Api.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private IDBService _dbService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IDBService dbService, IHttpContextAccessor httpContextAccessor)
        {
            _dbService = dbService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var query = new Query()
            {
                Procedure = UserProcedures.FindByUsername,
                Parameters = new Dictionary<string, object>()
                {
                    { "Username", username }
                },
                ExpectedType = typeof(User)
            };
            var user = (User)(await _dbService.ExecuteQuery(query))[0];

            if(user == null)
            {
                throw new KompanionException(KompanionErrors.ACCOUNT_NOT_FOUND.Item1, KompanionErrors.ACCOUNT_NOT_FOUND.Item2);
            }

            if(user.Status != (Int32)EntityStatus.Active)
            {
                throw new KompanionException(KompanionErrors.ACCOUNT_NOT_AUTHORIZED.Item1, KompanionErrors.ACCOUNT_NOT_AUTHORIZED.Item2);
            }

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new KompanionException(KompanionErrors.ACCOUNT_NOT_AUTHORIZED.Item1, KompanionErrors.ACCOUNT_NOT_AUTHORIZED.Item2);
            }

            return new User();
        }

        public int GetUserIdFromClaims()
        {
            var userId = default(Int32);

            try
            {
                userId = Int32.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

                return userId;
            }
            catch(Exception)
            {
                return userId;
            }
        }

        public User CreatePasswordHashAndSalt(User user, string password)
        {
            byte[] hash, salt;
            CreatePasswordHash(password, out hash, out salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            return user;
        }

        public async Task<string> CreateUserClaims(int userId)
        {
            var query = new Query
            {
                Procedure = UserProcedures.Read,
                Parameters = new Dictionary<string, object>()
                {
                    { "id", userId }
                },
                ExpectedType = typeof(User)
            };

            var user = (User)(await _dbService.ExecuteQuery(query))[0];

            if(user == null) 
            {
                throw new KompanionException(KompanionErrors.ACCOUNT_NOT_FOUND.Item1, KompanionErrors.ACCOUNT_NOT_FOUND.Item2);
            }

            var claims = new[] 
            {
                new Claim("UserId", userId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KompanionTopSecretTotallyNoNeedToMoveThisToKeyVaultAlsoPleaseHireMeThanks"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("Kompanion", "KompanionIssuer", claims, expires: DateTime.Now.AddDays(30), signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if(string.IsNullOrWhiteSpace(password))
            {
                throw new KompanionException(KompanionErrors.ACCOUNT_INVALID_PASSWORD.Item1, KompanionErrors.ACCOUNT_INVALID_PASSWORD.Item2);
            }

            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            if(password == null)
            {
                throw new ArgumentNullException("password");
            }

            if(string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if(hash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if(salt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");
            }

            using(var hmac = new HMACSHA512(salt)){
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != hash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}