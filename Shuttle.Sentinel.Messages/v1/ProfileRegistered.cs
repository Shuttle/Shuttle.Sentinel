using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class ProfileRegistered
    {
        public Guid Id { get; set; }
        public string EMailAddress { get; set; }
    }
}