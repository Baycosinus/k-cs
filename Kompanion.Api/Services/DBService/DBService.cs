using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Kompanion.Api.Helpers;
using Kompanion.Api.Infrastructure.DTO;
using Kompanion.Api.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Kompanion.Api.Services.DBService
{
    public class DBService: IDBService
    {
        private static string ConnectionString;

        public DBService(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<object>> ExecuteQuery(Query query)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(query.Procedure, connection);
            command.CommandType = CommandType.StoredProcedure;

            foreach(var keyValue in query.Parameters)
            {
                command.Parameters.AddWithValue(keyValue.Key, keyValue.Value);
            }

            MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            var result = new QueryResult()
            {
                Data = new List<Dictionary<string, object>>(),
            };

            while(reader.Read())
            {
                var row = new Dictionary<string, object>();
                for(var i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader.GetValue(i));
                }

                result.Data.Add(row);
            }

            result.Success = true;
            result.ExpectedType = query.ExpectedType;
            result.QueryType = query.QueryType;

            if(query.QueryType == QueryType.Read)
            {
                result.Data = result.Data.Take(1).ToList();
            }

            
            return DbHelper.MapResponseData(result);
        }
    }
}