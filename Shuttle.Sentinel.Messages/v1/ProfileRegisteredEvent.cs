using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class ProfileRegisteredEvent
    {
        public Guid Id { get; set; }
        public string EMailAddress { get; set; }
    }
}