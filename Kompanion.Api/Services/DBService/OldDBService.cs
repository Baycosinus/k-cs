using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Kompanion.Api.Models.Entities;
using Kompanion.Api.Helpers;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Kompanion.Api.Infrastructure.Exceptions;
using Kompanion.Api.Infrastructure.Enums;

namespace Kompanion.Api.Services.OldDBService
{
    public class OldDBService 
    {
        private static string ConnectionString;

        public OldDBService(IConfiguration configuration) 
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<object> Read(int id, Type type) 
        {
            var result = Activator.CreateInstance(type);
            var parameters = new Dictionary<string, string>()
            {
                    {"@Id", id.ToString()}
            };
            
            try
            {
                result = (await ExecuteQuery(DbHelper.GetProcedure(type, DbProcedureType.Read), parameters, type)).FirstOrDefault();
            }
            catch(Exception)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            return result;
        }

        public async Task<List<object>> List(Type type) 
        {
            var result = new List<object>();
            var parameters = new Dictionary<string, string>();
            try
            {
                result = await ExecuteQuery(DbHelper.GetProcedure(type, DbProcedureType.List), parameters, type);
            }
            catch (Exception)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            return result;
        }

        public async Task<object> Write(Type type, BaseModel data) 
        {
            var result = new List<object>();
            try
            {
                result = await ExecuteCommand(DbHelper.GetProcedure(type, DbProcedureType.Create), data, type);
            }
            catch(Exception)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            return result[0];
        }

        public async Task<object> Update(int id, Type type, BaseModel data) 
        {
            var result = new List<object>();
            try
            {
                result = await ExecuteCommand(DbHelper.GetProcedure(type, DbProcedureType.Update), data, type);
            }
            catch(Exception)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            return result;
        }
        

        public async Task<object> FindUserByUserName(string username)
        {
           var result = new User();
            var parameters = new Dictionary<string, string>()
            {
                    {"@UName", username}
            };
            
            try
            {
                result = (User)(await ExecuteQuery(UserProcedures.FindByUsername, parameters, typeof(User))).FirstOrDefault();
            }
            catch(Exception)
            {
                throw new KompanionException(KompanionErrors.DB_ERROR.Item1, KompanionErrors.DB_ERROR.Item2);
            }

            return result;
        }


        private static async Task<List<object>> ExecuteQuery(string proc, Dictionary<string, string> parameters, Type type)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(proc, connection);
            command.CommandType = CommandType.StoredProcedure;

            foreach(var keyValue in parameters)
            {
                command.Parameters.AddWithValue(keyValue.Key, keyValue.Value);
            }

            MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
            return MapResult(type, reader).Select(x => (object)x).ToList();
        }

        private static async Task<List<object>> ExecuteCommand(string proc, BaseModel data, Type type)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            MySqlCommand command = new MySqlCommand(proc, connection);
            command.CommandType = CommandType.StoredProcedure;

            var props = type.GetProperties();

            foreach(var prop in props)
            {
                var value = prop.GetValue(data);
                command.Parameters.AddWithValue(prop.Name, value);
            }

            MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();
            return MapResult(type, reader).Select(x => (object)x).ToList();
        }

        private static List<object> MapResult(Type type, MySqlDataReader reader)
        {
            var result = new List<object>();
            var props = type.GetProperties();
            while(reader.Read())
            {
                var row = Activator.CreateInstance(type);
                for(var i = 0; i < reader.FieldCount; i++)
                {
                    if(reader.GetValue(i) != DBNull.Value)
                    {
                        row.GetType().GetProperty(props[i].Name).SetValue(row, reader.GetValue(i));
                    }
                }
                result.Add((BaseModel)row);
            }

            return result;
        }
    }
}