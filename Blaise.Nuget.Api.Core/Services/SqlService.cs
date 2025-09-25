namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using MySql.Data.MySqlClient;

    public class SqlService : ISqlService
    {
        public IEnumerable<string> GetCaseIds(string connectionString, string questionnaireName)
        {
            ValidateInputs(connectionString, questionnaireName);

            var caseIds = new List<string>();
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT `{SqlFieldType.CaseId.FullName()}` FROM `{databaseTableName}`";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var caseId = reader[0]?.ToString();
                        if (!string.IsNullOrEmpty(caseId))
                        {
                            caseIds.Add(caseId);
                        }
                    }
                }
            }

            return caseIds;
        }

        public IEnumerable<string> GetEditingCaseIds(string connectionString, string questionnaireName)
        {
            ValidateInputs(connectionString, questionnaireName);

            var caseIds = new List<string>();
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);
            var databaseUneditedTableName = GetDatabaseTableNameUneditedForm(questionnaireName);

            if (!TableExists(connectionString, databaseUneditedTableName))
            {
                return caseIds;
            }

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT QUESTIONNAIRE.`{SqlFieldType.CaseId.FullName()}` " +
                                  $"FROM `{databaseTableName}` QUESTIONNAIRE " +
                                  $"JOIN `{databaseUneditedTableName}` UNEDITED " +
                                  $"ON QUESTIONNAIRE.`{SqlFieldType.CaseId.FullName()}` = UNEDITED.`{SqlFieldType.CaseId.FullName()}` " +
                                  $"AND (QUESTIONNAIRE.`{SqlFieldType.Edited.FullName()}` = 1 " +
                                  $"OR (QUESTIONNAIRE.`{SqlFieldType.EditLastUpdated.FullName()}` IS NULL AND UNEDITED.`{SqlFieldType.EditLastUpdated.FullName()}` IS NULL) " +
                                  $"OR (QUESTIONNAIRE.`{SqlFieldType.EditLastUpdated.FullName()}` = UNEDITED.`{SqlFieldType.EditLastUpdated.FullName()}`))";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var caseId = reader[0]?.ToString();
                        if (!string.IsNullOrEmpty(caseId))
                        {
                            caseIds.Add(caseId);
                        }
                    }
                }
            }

            return caseIds;
        }

        public IEnumerable<CaseIdentifierModel> GetCaseIdentifiers(string connectionString, string questionnaireName)
        {
            ValidateInputs(connectionString, questionnaireName);

            var caseIdentifiers = new List<CaseIdentifierModel>();
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT `{SqlFieldType.CaseId.FullName()}`, `{SqlFieldType.PostCode.FullName()}` FROM `{databaseTableName}`";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var caseId = reader[0]?.ToString() ?? string.Empty;
                        var postCode = reader[1]?.ToString() ?? string.Empty;
                        caseIdentifiers.Add(new CaseIdentifierModel(caseId, postCode));
                    }
                }
            }

            return caseIdentifiers;
        }

        public string GetPostCode(string connectionString, string questionnaireName, string primaryKey)
        {
            ValidateInputs(connectionString, questionnaireName);

            if (string.IsNullOrWhiteSpace(primaryKey))
            {
                throw new ArgumentException("Primary key cannot be null or empty", nameof(primaryKey));
            }

            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);

            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT `{SqlFieldType.PostCode.FullName()}` FROM `{databaseTableName}` WHERE `{SqlFieldType.CaseId.FullName()}` = @primaryKey";
                cmd.Parameters.AddWithValue("@primaryKey", primaryKey);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException($"No record found for primary key: {primaryKey}");
                    }

                    return reader[0]?.ToString() ?? string.Empty;
                }
            }
        }

        public bool DropQuestionnaireTables(string connectionString, string questionnaireName)
        {
            ValidateInputs(connectionString, questionnaireName);

            // implemented this way as StatNeth don't currently provide a way to drop the SQL tables via the API
            var firstDatabaseTableName = GetDatabaseTableNameForm(questionnaireName);
            var secondDatabaseTableName = GetDatabaseTableNameDml(questionnaireName);

            try
            {
                using (var con = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;

                    cmd.CommandText = $"DROP TABLE IF EXISTS `{firstDatabaseTableName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"DROP TABLE IF EXISTS `{secondDatabaseTableName}`";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException mySqlException)
            {
                Console.WriteLine($"Failed to drop questionnaire tables for '{questionnaireName}': {mySqlException}");
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to drop questionnaire tables for '{questionnaireName}': {exception}");
                return false;
            }

            return true;
        }

        private static void ValidateInputs(string connectionString, string questionnaireName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(questionnaireName))
            {
                throw new ArgumentException("Questionnaire name cannot be null or empty", nameof(questionnaireName));
            }
        }

        private static string GetDatabaseTableNameUneditedForm(string questionnaireName)
        {
            return $"{questionnaireName.Replace("_EDIT", string.Empty)}_Form";
        }

        private static string GetDatabaseTableNameForm(string questionnaireName)
        {
            return $"{questionnaireName}_Form";
        }

        private static string GetDatabaseTableNameDml(string questionnaireName)
        {
            return $"{questionnaireName}_Dml";
        }

        private bool TableExists(string connectionString, string databaseTableName)
        {
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";
                cmd.Parameters.AddWithValue("@tableName", databaseTableName);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]) > 0;
                    }
                }
            }

            return false;
        }
    }
}
