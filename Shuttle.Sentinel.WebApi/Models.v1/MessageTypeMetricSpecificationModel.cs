using System;

namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class MessageTypeMetricSpecificationModel
    {
        public DateTime StartDateRegistered { get; set; }
        public string MessageTypeMatch { get; set; }
    }
}