namespace Blaise.Nuget.Api.Contracts.Models
{
    using System;
    using System.Collections.Generic;

    public class DayBatchModel
    {
        public DayBatchModel()
        {
            CaseIds = new List<string>();
        }

        public DayBatchModel(DateTime dayBatchDate, List<string> caseIds)
        {
            DayBatchDate = dayBatchDate;
            CaseIds = caseIds;
        }

        public DateTime DayBatchDate { get; set; }

        public List<string> CaseIds { get; set; }
    }
}
