using System;
using System.Collections.Generic;
using Kompanion.Api.Infrastructure.Enums;

namespace Kompanion.Api.Infrastructure.DTO
{
    public class QueryResult
    {
        public bool Success { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
        public Type ExpectedType { get; set; }
        public QueryType QueryType { get; set; }
    }
}