using System;

namespace Shuttle.Sentinel.WebApi
{
    public class SetRolePermissionModel
    {
        public Guid RoleId { get; set; }
        public string Permission { get; set; }
        public bool Active { get; set; }
    }
}