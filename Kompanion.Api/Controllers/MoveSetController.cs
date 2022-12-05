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
    public class MoveSetController : BaseController
    {
        private readonly ILogger<MoveSetController> _logger;
        private readonly IDBService _dbService;
        private readonly IIdentityService _identityService;

        public MoveSetController(ILogger<MoveSetController> logger, IDBService dbService, IIdentityService identityService)
        {
            _logger = logger;
            _dbService = dbService;
            _identityService = identityService;
        }

        [HttpGet]
        [Route("movesets")]
        public async Task<IActionResult> List()
        {
            var query = new Query
            {
                Procedure = MoveSetProcedures.List,
                Parameters = new Dictionary<string, object>(),
                ExpectedType = typeof(MoveSet),
                QueryType = QueryType.List
            };

            var result = await _dbService.ExecuteQuery(query);
            return ResponseOk(result);
        }

        [HttpGet]
        [Route("movesets/{id}")]
        public async Task<IActionResult> Read(int id)
        {
            var query = new Query
            {
                Procedure = MoveSetProcedures.Read,
                Parameters = new Dictionary<string, object>()
                {
                    { "id", id }
                },
                ExpectedType = typeof(MoveSet),
                QueryType = QueryType.Read
            };

            var result = await _dbService.ExecuteQuery(query);
            return ResponseOk(result);
        }

        [Authorize]
        [HttpPost]
        [Route("movesets")]
        public async Task<IActionResult> Create([FromBody] MoveSetDTO moveSet)
        {
            var userId = _identityService.GetUserIdFromClaims();
            // Request validation middleware (Validate + Sanitize)

            var newMoveSet = new MoveSet
            {
                Name = moveSet.Name,
                Description = moveSet.Description,
                CreateDate = DateTime.UtcNow,
                Status = (int)EntityStatus.Active,
                CreatorUserId = userId
            };

            var query = new Query
            {
                Procedure = MoveSetProcedures.Create,
                Parameters = new Dictionary<string, object>()
                {
                    { "name", newMoveSet.Name },
                    { "description", newMoveSet.Description },
                    { "createDate", newMoveSet.CreateDate },
                    { "status", newMoveSet.Status },
                    { "creatorUserId", newMoveSet.CreatorUserId }
                },
                ExpectedType = typeof(MoveSet),
                QueryType = QueryType.Insert
            };

            var result = await _dbService.ExecuteQuery(query);

            return ResponseCreated(result);
        }

        [Authorize]
        [HttpPut]
        [Route("movesets/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MoveSetDTO moveSet)
        {
            var userId = _identityService.GetUserIdFromClaims();
            // Request validation middleware (Validate + Sanitize)

            // Check if user is the creator of the moveset OR an admin
            var newMoveSet = new MoveSet
            {
                Name = moveSet.Name,
                Description = moveSet.Description,
                CreateDate = DateTime.UtcNow,
                Status = moveSet.Status, //Status Validation needed.
                UpdaterUserId = userId,
                UpdateDate = DateTime.UtcNow
            };

            var query = new Query
            {
                Procedure = MoveSetProcedures.Update,
                Parameters = new Dictionary<string, object>()
                {
                    { "id", id },
                    { "name", newMoveSet.Name },
                    { "description", newMoveSet.Description },
                    { "updateDate", newMoveSet.UpdateDate },
                    { "updaterUserId", newMoveSet.UpdaterUserId },
                    { "status", newMoveSet.Status }
                },
                ExpectedType = typeof(MoveSet),
                QueryType = QueryType.Update
            };

            var result = await _dbService.ExecuteQuery(query);

            return ResponseCreated(result);
        }


    }
}