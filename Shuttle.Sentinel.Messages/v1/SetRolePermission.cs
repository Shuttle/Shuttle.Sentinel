using System;

namespace Shuttle.Sentinel.Messages.v1
{
    public class SetRolePermission
    {
        public Guid RoleId { get; set; }
        public string Permission { get; set; }
        public bool Active { get; set; }
    }
}