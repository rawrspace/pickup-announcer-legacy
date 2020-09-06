using Dapper;
using PickupAnnouncerLegacy.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Services
{
    public class DbService : IDbService
    {
        private readonly string _connectionString;
        private readonly int COMMAND_TIMEOUT = 9000;

        public DbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<T>> Get<T>(string whereConditions = null, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return await connection.GetListAsync<T>(whereConditions, parameters);
            }
        }

        public async Task Insert<T>(IEnumerable<T> itemsToInsert)
        {
            using (var connection = GetConnection())
            {
                foreach (var item in itemsToInsert)
                {
                    await connection.InsertAsync(item);
                }
            }
        }

        public async Task<int?> Insert<T>(T itemToInsert)
        {
            using (var connection = GetConnection())
            {
                return await connection.InsertAsync(itemToInsert);
            }
        }

        public async Task<int> Update<T>(T itemToUpdate)
        {
            using (var connection = GetConnection())
            {
                return await connection.UpdateAsync(itemToUpdate);
            }
        }

        public async Task<int> Delete<T>(int id)
        {
            using (var connection = GetConnection())
            {
                return await connection.DeleteAsync<T>(id);
            }
        }

        public async Task<IList<IDictionary<string, object>>> ExecuteQuery(string queryText, IDictionary<string, object> parameters) => await ExecuteSQL(CommandType.Text, queryText, parameters);

        public async Task<IList<IDictionary<string, object>>> ExecuteStoredProcedure(string sprocName, IDictionary<string, object> parameters = null) => await ExecuteSQL(CommandType.StoredProcedure, sprocName, parameters);

        private async Task<T> ExecuteDbCommand<T>(Func<DbCommand, Task<T>> func)
        {
            using (var cn = GetConnection())
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandTimeout = COMMAND_TIMEOUT;
                    return await func(cmd);
                }
            }
        }

        protected async Task<IList<IDictionary<string, object>>> ExecuteSQL(CommandType commandType, string sqlText, IDictionary<string, object> parameters)
        {
            return await ExecuteDbCommand(async (cmd) =>
            {
                var results = new List<IDictionary<string, object>>();
                cmd.CommandText = sqlText;
                cmd.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var (key, value) in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter('@' + key, value));
                    }
                }


                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        results.Add(ParseDynamicRecord(reader));
                    }
                }

                return results;
            });
        }

        protected static IDictionary<string, object> ParseDynamicRecord(DbDataReader reader)
        {
            var objDictionary = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
                objDictionary.Add(reader.GetName(i), reader[i]);
            return objDictionary;
        }
    }
}
