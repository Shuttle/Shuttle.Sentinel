using System;
using Shuttle.Core.Data;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeMetricQueryFactory
    {
        IQuery Search(MessageTypeMetric.Specification specification);
        IQuery Register(Guid metricId, string messageType, DateTime dateRegistered, Guid endpointId, int count, double fastestExecutionDuration, double slowestExecutionDuration, double totalExecutionDuration);
    }
}