﻿using System;
using System.Collections.Generic;
using Shuttle.Sentinel.DataAccess.Query;

namespace Shuttle.Sentinel.DataAccess
{
    public interface IMessageTypeMetricQuery
    {
        IEnumerable<MessageTypeMetric> Search(MessageTypeMetric.Specification specification);
        void Register(Guid transportMessageMessageId, string metricMessageType, DateTime transportMessageSendDate, Guid endpointId, int metricCount, double metricFastestExecutionDuration, double metricSlowestExecutionDuration, double metricTotalExecutionDuration);
    }
}