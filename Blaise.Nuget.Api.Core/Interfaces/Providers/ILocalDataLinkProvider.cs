﻿
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface ILocalDataLinkProvider
    {
        IDataLink GetDataLink(string fullFilePath);
    }
}
