using System;

namespace Shuttle.Sentinel.WebApi.Models.v1
{
    public class SetRolePermissionModel
    {
        public Guid RoleId { get; set; }
        public string Permission { get; set; }
        public bool Active { get; set; }
    }
}