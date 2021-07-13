using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class SurveyMetaService : ISurveyMetaService
    {
        private readonly IDataModelService _dataModelService;

        public SurveyMetaService(IDataModelService dataModelService)
        {
            _dataModelService = dataModelService;
        }

        public IEnumerable<string> GetSurveyModes(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName) as IDatamodel2;

            return dataModel == null ? new List<string>() : dataModel.Modes.Select(dm => dm.Name);
        }
    }
}