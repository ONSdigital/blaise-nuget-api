﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseAuditTrailApi
    {
        void GetAuditTrail(string instrumentId, string sessionId);
    }
}
