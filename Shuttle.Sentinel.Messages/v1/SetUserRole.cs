using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class SetUserRole
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }
}