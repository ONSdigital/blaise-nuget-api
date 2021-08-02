using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using MySql.Data.MySqlClient;

namespace Blaise.Nuget.Api.Core.Services
{
    public class MySqlService : IMySqlService
    {
        public IEnumerable<string> GetCaseIds(string connectionString, string instrumentName)
        {
            var caseIds = new List<string>();
            var databaseTableName = $"{instrumentName}_Form";
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT Serial_Number from {databaseTableName}";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        caseIds.Add(reader[0].ToString());
                    }
                }

                con.Close();
            }

            return caseIds;
        }
    }
}
