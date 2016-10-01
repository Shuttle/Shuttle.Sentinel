using System;
using System.Collections.Generic;

namespace Shuttle.Sentinel.WebApi
{
    public class UserRoleStatusModel
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}