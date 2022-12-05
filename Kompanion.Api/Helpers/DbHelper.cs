using System;
using System.Collections.Generic;
using System.Linq;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Infrastructure.Enums;
using Kompanion.Api.Infrastructure.Exceptions;
using Kompanion.Api.Models.Entities;

namespace Kompanion.Api.Helpers
{
    public static class DbHelper
    {
        public static Dictionary<Type, Type> TypeProcedure = new Dictionary<Type, Type>()
        {
            { typeof(User), typeof(UserProcedures) },
            { typeof(Move), typeof(MoveProcedures) },
            { typeof(MoveSet), typeof(MoveSetProcedures) }
        };

        public static string GetProcedure(Type entityType, DbProcedureType procedureType)
        {
            return TypeProcedure[entityType].GetField(procedureType.ToString()).GetValue(null).ToString();
        }

        public static List<object> MapResponseData(QueryResult queryResult)
        {
            if (!queryResult.Success)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            var result = new List<object>();

            try
            {
                foreach (var row in queryResult.Data)
                {
                    var item = Activator.CreateInstance(queryResult.ExpectedType);
                    foreach (var column in row)
                    {
                        var property = queryResult.ExpectedType.GetProperty(column.Key);
                        if(column.Value != DBNull.Value)
                        {
                            property.SetValue(item, column.Value);
                        }
                        
                    }

                    result.Add(item);
                }

            }
            catch (Exception)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            return result;
        }

    }
}