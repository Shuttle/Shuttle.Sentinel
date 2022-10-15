using System;

namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class SetUserRoleModel
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }
}