﻿using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Models
{
    public class OpenConnectionModel
    {
        public string ConnectionType { get; set; }

        public int Connections { get; set; }

        public List<DateTime> ExpirationDateTimes { get; set; }
    }
}
