namespace Blaise.Nuget.Api.Core.Services
{
    using System;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Enums;
    using Blaise.Nuget.Api.Contracts.Extensions;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using MySql.Data.MySqlClient;

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
            var commandText = $"SELECT QUESTIONNAIRE.{SqlFieldType.CaseId.FullName()} " +
                                      $"FROM {databaseTableName} QUESTIONNAIRE " +
                                      $"JOIN {databaseUneditedTableName} UNEDITED " +
                                      $"ON QUESTIONNAIRE.{SqlFieldType.CaseId.FullName()} = UNEDITED.{SqlFieldType.CaseId.FullName()} " +
                                      $"AND (QUESTIONNAIRE.{SqlFieldType.Edited.FullName()} = 1 " +
                                      $"OR (QUESTIONNAIRE.{SqlFieldType.EditLastUpdated.FullName()} IS NULL AND UNEDITED.{SqlFieldType.EditLastUpdated.FullName()} IS NULL) " +
                                      $"OR (QUESTIONNAIRE.{SqlFieldType.EditLastUpdated.FullName()} = UNEDITED.{SqlFieldType.EditLastUpdated.FullName()}))";


            try
            {
                if (!TableExists(connectionString, databaseUneditedTableName))
                {
                    return caseIds;
                }

                using (var con = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT QUESTIONNAIRE.{SqlFieldType.CaseId.FullName()} " +
                                      $"FROM {databaseTableName} QUESTIONNAIRE " +
                                      $"JOIN {databaseUneditedTableName} UNEDITED " +
                                      $"ON QUESTIONNAIRE.{SqlFieldType.CaseId.FullName()} = UNEDITED.{SqlFieldType.CaseId.FullName()} " +
                                      $"AND (QUESTIONNAIRE.{SqlFieldType.Edited.FullName()} = 1 " +
                                      $"OR (QUESTIONNAIRE.{SqlFieldType.EditLastUpdated.FullName()} IS NULL AND UNEDITED.{SqlFieldType.EditLastUpdated.FullName()} IS NULL) " +
                                      $"OR (QUESTIONNAIRE.{SqlFieldType.EditLastUpdated.FullName()} = UNEDITED.{SqlFieldType.EditLastUpdated.FullName()}))";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            caseIds.Add(reader[0].ToString());
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[SqlService.GetEditingCaseIds] Exception: {ex.Message} | ConnectionString: {connectionString} | QuestionnaireName: {questionnaireName} | CommandText: {commandText}");
                throw;
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
            var databaseTableName = GetDatabaseTableNameForm(questionnaireName);
            using (var con = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand())
            {
                con.Open();
                cmd.CommandText = $"SELECT {SqlFieldType.PostCode.FullName()} from `{databaseTableName}` WHERE {SqlFieldType.CaseId.FullName()} = @primaryKey";
                cmd.Parameters.AddWithValue("@primaryKey", primaryKey);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.IsDBNull(0) ? null : reader.GetString(0);
                    }
                }
            }

            return null;
        }

        public bool DropQuestionnaireTables(string connectionString, string questionnaireName)
        {
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
            catch
            {
                return false;
            }

            return true;
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
                cmd.CommandText = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";
                cmd.Parameters.AddWithValue("@tableName", databaseTableName);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader[0].ToString() == "1";
                    }
                }
            }

            return false;
        }
    }
}
