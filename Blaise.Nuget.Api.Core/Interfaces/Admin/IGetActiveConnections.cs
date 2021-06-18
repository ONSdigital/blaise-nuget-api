﻿using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces.Admin
{
    public interface IGetOpenConnections
    {
        int GetNumberOfOpenConnections();

        Dictionary<string, DateTime> GetConnections();
    }
}