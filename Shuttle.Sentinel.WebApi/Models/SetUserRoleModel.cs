using System;

namespace Shuttle.Sentinel.WebApi
{
    public class SetUserRoleModel
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }
}