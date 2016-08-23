using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;
using Shuttle.Sentinel.DomainEvents.Role.v1;

namespace Shuttle.Sentinel
{
    public class Role
    {
        private readonly Guid _id;
        private readonly List<string> _permissions = new List<string>();
        private string _name;

        public Role(Guid id)
        {
            _id = id;
        }

        public Added Add(string name)
        {
            return On(new Added
            {
                Name = name
            });
        }

        public Added On(Added added)
        {
            Guard.AgainstNull(added, "added");

            _name = added.Name;

            return added;
        }

        public static string Key(string name)
        {
            return string.Format("[role]:name={0};", name);
        }

        public PermissionAdded AddPermission(string permission)
        {
            return On(new PermissionAdded {Permission = permission});
        }

        public PermissionAdded On(PermissionAdded permissionAdded)
        {
            Guard.AgainstNull(permissionAdded, "permissionAdded");

            _permissions.Add(permissionAdded.Permission);

            return permissionAdded;
        }
    }
}