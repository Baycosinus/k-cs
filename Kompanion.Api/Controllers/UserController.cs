using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kompanion.Api.Helpers;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Infrastructure.Enums;
using Kompanion.Api.Models.Entities;
using Kompanion.Api.Services;
using Kompanion.Api.Services.DBService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kompanion.Api.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IDBService _dbService;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            _dbService = new DBService(_configuration);
        }

        [HttpGet]
        [Route("users/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new Query()
            {
                Procedure = UserProcedures.Read,
                Parameters = new Dictionary<string, object>()
                {
                    { "id", id }
                },
                ExpectedType = typeof(User)
            };
            var result = (User)(await _dbService.ExecuteQuery(query))[0];
            return ResponseCreated(result);
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> List()
        {
            var result = await _dbService.ExecuteQuery(new Query()
            {
                Procedure = UserProcedures.List,
                ExpectedType = typeof(User),
                QueryType = QueryType.List,
                Parameters = new Dictionary<string, object>()
            });

            return ResponseCreated(result);
        }
    }
}
