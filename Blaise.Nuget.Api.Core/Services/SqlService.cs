using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Services
{
    public class SqlService : ISqlService
    {
        public IEnumerable<string> GetCaseIds(string connectionString, string questionnaireName)
        {
            var caseIds = new List<string>();
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);
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

        public IEnumerable<string> GetEditingCaseIds(string connectionString, string questionnaireName)
        {
            var caseIds = new List<string>();
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);
            var databaseUneditedTableName = GetDatabaseTableNameUneditedForm(questionnaireName);
            
            if (!TableExists(databaseUneditedTableName))
            {
                return caseIds;
            }
            
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT {SqlFieldType.CaseId.FullName()}" +
                                  $"FROM {databaseTableName} QUESTIONNAIRE" +
                                  $"Join {databaseUneditedTableName} UNEDITED" +
                                  "ON QUESTIONNARE.Serial_Number = UNEDITED.Serial_Number";

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

        public IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string connectionString, string questionnaireName)
        {
            var caseIdentifiers = new List<CaseIdentifierModel>();
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);
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

        public string GetPostCode(string connectionString, string questionnaireName, string primaryKey)
        {
            string postCode;
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
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

        public bool DropQuestionnaireTables(string connectionString, string questionnaireName)
        {
            /*Had to implement it this way as StatsNeth have no functionality to achieve the same result*/
            var firstDatabaseTableName = GetDatabaseTableNameForm(questionnaireName);
            var secondDatabaseTableName = GetDatabaseTableNameDml(questionnaireName);

            try
            {
                using (var con = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;

                    // Drop first table
                    cmd.CommandText = $"DROP TABLE IF EXISTS `{firstDatabaseTableName}`";
                    cmd.ExecuteNonQuery();

                    // Drop second table
                    cmd.CommandText = $"DROP TABLE IF EXISTS `{secondDatabaseTableName}`";
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                // Handle exception
                return false;
            }

            return true;
        }

        private bool TableExists(string connectionString, string databaseTableName)
        {
            bool tableExists;
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{databaseTableName}'";

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    tableExists = reader[0].ToString() == "1";
                }

                con.Close();
            }

            return tableExists;
        }

        private static string GetDatabaseTableNameForm(string questionnaireName)
        {
            return $"{questionnaireName}_Form";
        }

        private static string GetDatabaseTableNameUneditedForm(string questionnaireName)
        {
            return $"{questionnaireName}_UNEDITED_Form";
        }

        private static string GetDatabaseTableNameDml(string questionnaireName)
        {
            return $"{questionnaireName}_Dml";
        }
    }
}
