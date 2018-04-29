using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeMetricQueryFactory
    {
        IQuery Search(DateTime @from, string match);
    }
}