using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using ATA = StatNeth.Blaise.API.AuditTrail;
// ReSharper disable All

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseAuditTrailApi : IBlaiseAuditTrailApi
    {
        private readonly IQuestionnaireService _questionnaireService;
        private ConnectionModel _connectionModel;

        public BlaiseAuditTrailApi(ConnectionModel connection)
        {
            _questionnaireService = UnityProvider.Resolve<IQuestionnaireService>();
            _connectionModel = connection;
        }

        public byte[] GetAuditTrail(string serverPark, string questionnaireName)
        {
            if (string.IsNullOrEmpty(serverPark) || string.IsNullOrEmpty(questionnaireName))
                throw new ArgumentNullException($"Error - Expected paramter is empty, ServerPark: {serverPark}, QuestionnaireName: {questionnaireName}");

            //***************************************************************
            //Get the Blaise configuration
            //***************************************************************
            var configurationProvider = UnityProvider.Resolve<IBlaiseConfigurationProvider>();

            if (_connectionModel is null || string.IsNullOrEmpty(_connectionModel.ServerName))
                _connectionModel = configurationProvider.GetConnectionModel();

            //Check the connection details are in the correct ranges
            if (!_connectionModel.Binding.Equals("http") || _connectionModel.Binding.Equals("https"))
                throw new InvalidOperationException("Invalid configuration: The Blaise configuration is invalid");

            //***************************************************************
            //Get the questionnaireid from the questionnaire name
            //***************************************************************
            var instrumentId = _questionnaireService.GetQuestionnaireId(_connectionModel, questionnaireName, serverPark);

            var ras =
               ATA.AuditTrailManager.GetRemoteAuditTrailServer(
                   configurationProvider.GetConnectionModel().ServerName,
                   configurationProvider.GetConnectionModel().Port,
                   configurationProvider.GetConnectionModel().UserName,
                   GetPassword(configurationProvider.GetConnectionModel().Password));

            var auditEvents =
                ras.GetInstrumentEvents(instrumentId, serverPark) as ATA.IInstrumentEvents2;

            if (auditEvents != null)
            {
                //***********************************************************
                //Get the keys
                //***********************************************************
                var listOfAuditEvents = new List<AuditTrailData>();
                var keyValues = auditEvents.GetKeyValues();

                foreach (var keyVal in keyValues)
                {
                    //***********************************************************
                    //For each key get the session Ids associated 
                    //***********************************************************
                    var sessionIdAttachedToKey = auditEvents.GetSessionIDsByKeyValue(keyVal);

                    foreach (var sessionId in sessionIdAttachedToKey)
                    {
                        var sessionEvents = auditEvents.GetSessionEvents(sessionId);

                        foreach (var sessionEvent in sessionEvents.Events)
                        {
                            var auditTrailData = new AuditTrailData
                            {
                                KeyValue = keyVal,
                                SessionId = sessionId,
                                TimeStamp = sessionEvent.TimeStamp,
                                Content = sessionEvent.ToString()
                            };
                            listOfAuditEvents.Add(auditTrailData);
                        }
                    }
                }

                if (listOfAuditEvents.Any())
                {
                    var csvContent = GenerateCsvContent(listOfAuditEvents);
                    return GenerateFileInMemory(csvContent);
                }
            }

            return null;
        }

        private static SecureString GetPassword(string pw)
        {
            var password = new SecureString();

            foreach (var character in pw)
            {
                password.AppendChar(character);
            }
            return password;
        }

        private string GenerateCsvContent(List<AuditTrailData> listOfEvents)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("KeyValue,SessionId,Timestamp,Content");

            foreach (var eventFromAudit in listOfEvents.OrderBy(o => o.KeyValue))
            {
                csvContent.AppendLine($"{eventFromAudit.KeyValue}, {eventFromAudit.SessionId}, {eventFromAudit.TimeStamp.ToString("dd/MM/yyyy HH:mm:ss")}, {eventFromAudit.Content}");
            }
            return csvContent.ToString();
        }

        private byte[] GenerateFileInMemory(string csvContent)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(csvContent);
                    streamWriter.Flush();

                    // Rewind the MemoryStream
                    memoryStream.Position = 0;

                    //Save as byte array
                    return memoryStream.ToArray();

                    //For testing the output for Dave K - will remove once format is signed off
                    /*  using (var streamReader = new StreamReader(memoryStream))
                      {
                          var csvContentFromMemory = streamReader.ReadToEnd();
                          File.WriteAllText(@"c:\temp\output.csv", csvContentFromMemory);
                      }*/
                }
            }
        }
    }
}

