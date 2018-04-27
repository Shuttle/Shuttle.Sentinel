using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi
{
    [Route("api/[controller]")]
    public class EndpointsController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public EndpointsController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory, IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(bus, nameof(bus));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _bus = bus;
        }

        [RequiresPermission(SystemPermissions.Manage.Endpoints)]
        [HttpGet]
        public IActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = Data(_endpointQuery.All())
                });
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Endpoints)]
        [HttpGet("{search}")]
        public IActionResult GetSearch(string search)
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(new
                {
                    Data = Data(_endpointQuery.Search(search))
                });
            }
        }

        private IEnumerable<dynamic> Data(IEnumerable<Endpoint> endpoints)
        {
            var result = new List<dynamic>();

            foreach (var endpoint in endpoints)
            {
                result.Add(new
                {
                    endpoint.Id,
                    endpoint.MachineName,
                    endpoint.BaseDirectory,
                    endpoint.EntryAssemblyQualifiedName,
                    ipv4Address = endpoint.IPv4Address,
                    endpoint.InboxWorkQueueUri,
                    endpoint.InboxDeferredQueueUri,
                    endpoint.InboxErrorQueueUri,
                    endpoint.ControlInboxWorkQueueUri,
                    endpoint.ControlInboxErrorQueueUri,
                    endpoint.OutboxWorkQueueUri,
                    endpoint.OutboxErrorQueueUri,
                    InboxWorkQueueUriSecured = GetSecuredUri(endpoint.InboxWorkQueueUri),
                    InboxDeferredQueueUriSecured = GetSecuredUri(endpoint.InboxDeferredQueueUri),
                    InboxErrorQueueUriSecured = GetSecuredUri(endpoint.InboxErrorQueueUri),
                    ControlInboxWorkQueueUriSecured = GetSecuredUri(endpoint.ControlInboxWorkQueueUri),
                    ControlInboxErrorQueueUriSecured = GetSecuredUri(endpoint.ControlInboxErrorQueueUri),
                    OutboxWorkQueueUriSecured = GetSecuredUri(endpoint.OutboxWorkQueueUri),
                    OutboxErrorQueueUriSecured = GetSecuredUri(endpoint.OutboxErrorQueueUri)
                });
            }

            return result;
        }

        private string GetSecuredUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return string.Empty;
            }

            try
            {
                return new Uri(uri).Secured().ToString();
            }
            catch
            {
                return "(invalid uri)";
            }
        }

        [RequiresPermission(SystemPermissions.Manage.Endpoints)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveEndpointCommand
            {
                Id = id
            });

            return Ok();
        }
    }
}