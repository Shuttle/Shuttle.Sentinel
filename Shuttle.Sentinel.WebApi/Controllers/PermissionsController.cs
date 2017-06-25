using System;
using System.Linq;
using System.Web.Http;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
    public class PermissionsController : SentinelApiController
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IPermissionQuery _permissionQuery;

        public PermissionsController(IDatabaseContextFactory databaseContextFactory, IPermissionQuery permissionQuery)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(permissionQuery, "permissionQuery");

            _databaseContextFactory = databaseContextFactory;
            _permissionQuery = permissionQuery;
        }

        public IHttpActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = _permissionQuery.Available()
                        .Select(permission => new
                        {
                            Permission = permission
                        }).ToList()
                });
            }
        }
    }
}