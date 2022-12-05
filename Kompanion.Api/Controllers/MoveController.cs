using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Infrastructure.Enums;
using Kompanion.Api.Models.Entities;
using Kompanion.Api.Services;
using Kompanion.Api.Services.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kompanion.Api.Controllers
{
    [ApiController]
    public class MoveController : BaseController
    {
        private readonly ILogger<MoveController> _logger;
        private readonly IDBService _dbService;
        private readonly IIdentityService _identityService;

        public MoveController(ILogger<MoveController> logger, IDBService dbService, IIdentityService identityService)
        {
            _logger = logger;
            _dbService = dbService;
            _identityService = identityService;
        }

        [HttpGet]
        [Route("moves")]
        public async Task<IActionResult> List()
        {
            var query = new Query
            {
                Procedure = MoveProcedures.List,
                Parameters = new Dictionary<string, object>(),
                ExpectedType = typeof(Move),
                QueryType = QueryType.List
            };

            var result = await _dbService.ExecuteQuery(query);
            return ResponseOk(result);
        }

        [HttpGet]
        [Route("moves/{id}")]
        public async Task<IActionResult> Read(int id)
        {
            var query = new Query
            {
                Procedure = MoveProcedures.Read,
                Parameters = new Dictionary<string, object>()
                {
                    { "id", id }
                },
                ExpectedType = typeof(Move),
                QueryType = QueryType.Read
            };

            var result = await _dbService.ExecuteQuery(query);
            return ResponseOk(result);
        }

        [Authorize]
        [HttpPost]
        [Route("moves")]
        public async Task<IActionResult> Create([FromBody] MoveDTO Move)
        {
            var userId = _identityService.GetUserIdFromClaims();
            // Request validation middleware (Validate + Sanitize)

            var newMove = new Move
            {
                Name = Move.Name,
                Description = Move.Description,
                CreateDate = DateTime.UtcNow,
                Status = (int)EntityStatus.Active,
                CreatorUserId = userId
            };

            var query = new Query
            {
                Procedure = MoveProcedures.Create,
                Parameters = new Dictionary<string, object>()
                {
                    { "name", newMove.Name },
                    { "description", newMove.Description },
                    { "createDate", newMove.CreateDate },
                    { "status", newMove.Status },
                    { "creatorUserId", newMove.CreatorUserId }
                },
                ExpectedType = typeof(Move),
                QueryType = QueryType.Insert
            };

            var result = await _dbService.ExecuteQuery(query);

            return ResponseCreated(result);
        }

        [Authorize]
        [HttpPut]
        [Route("moves/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MoveDTO move)
        {
            var userId = _identityService.GetUserIdFromClaims();
            // Request validation middleware (Validate + Sanitize)

            // Check if user is the creator of the moveset OR an admin
            var query = new Query
            {
                Procedure = MoveProcedures.Update,
                Parameters = new Dictionary<string, object>()
                {
                    { "id", id },
                    { "name", move.Name },
                    { "description", move.Description },
                    { "status", move.Status }, // Status validation needed.
                    { "updaterUserId", userId },
                    { "updateDate", DateTime.UtcNow }
                },
                ExpectedType = typeof(Move),
                QueryType = QueryType.Update
            };

            var result = await _dbService.ExecuteQuery(query);

            return ResponseCreated(result);
        }


    }
}