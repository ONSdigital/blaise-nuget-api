using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using MySql.Data.MySqlClient;

namespace Blaise.Nuget.Api.Core.Services
{
    public class SqlService : ISqlService
    {
        public IEnumerable<string> GetCaseIds(string connectionString, string instrumentName)
        {
            var caseIds = new List<string>();
            var databaseTableName = GetDatabaseTableName(instrumentName);
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT {SqlFieldType.CaseId.FullName()} from {databaseTableName}";

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

        public IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string connectionString, string instrumentName)
        {
            var caseIdentifiers = new List<CaseIdentifierModel>();
            var databaseTableName = GetDatabaseTableName(instrumentName);
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT {SqlFieldType.CaseId.FullName()}, {SqlFieldType.PostCode.FullName()} from {databaseTableName}";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        caseIdentifiers.Add(new CaseIdentifierModel(reader[0].ToString(), reader[1].ToString()));
                    }
                }

                con.Close();
            }

            return caseIdentifiers;
        }

        public string GetPostCode(string connectionString, string instrumentName, string primaryKey)
        {
            string postCode;
            var databaseTableName = GetDatabaseTableName(instrumentName);
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.Connection = con;
                cmd.CommandText = $"SELECT {SqlFieldType.PostCode.FullName()} from {databaseTableName} WHERE {SqlFieldType.CaseId.FullName()} = {primaryKey}";
                
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    postCode = reader[0].ToString();
                }

                con.Close();
            }

            return postCode;
        }

        private static string GetDatabaseTableName(string instrumentName)
        {
            return $"{instrumentName.ToUpper()}_Form";
        }
    }
}
