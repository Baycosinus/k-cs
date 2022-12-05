using System;
using System.Collections.Generic;
using Kompanion.Api.Infrastructure.Enums;

namespace Kompanion.Api.Infrastructure.DTO
{
    public class Query
    {
       public string Procedure { get; set; }
       public QueryType QueryType { get; set; }
       public Dictionary<string, object> Parameters { get; set; }
       public Type ExpectedType { get; set; }
    }
}