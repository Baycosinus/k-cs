using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Models;
using Kompanion.Api.Models.Entities;

namespace Kompanion.Api.Services{
    public interface IDBService
    {
        Task<List<object>> ExecuteQuery(Query query);
    }
}