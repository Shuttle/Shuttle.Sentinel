using System;
using Shuttle.Core.Data;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeMetricQueryFactory
    {
        IQuery Search(DateTime @from, string match);
        IQuery Register(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId, int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration);
    }
}