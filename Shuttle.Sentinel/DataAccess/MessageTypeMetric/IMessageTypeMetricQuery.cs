using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeMetricQuery
    {
        IEnumerable<MessageTypeMetric> Search(DateTime @from, string match);
    }
}