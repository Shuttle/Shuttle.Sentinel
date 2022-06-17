using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;
using Shuttle.Sentinel.WebApi.Configuration;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class EndpointsController : Controller
    {
        private readonly IServiceBus _bus;
        private readonly ISentinelConfiguration _configuration;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;

        public EndpointsController(IServiceBus bus, IDatabaseContextFactory databaseContextFactory,
            IEndpointQuery endpointQuery, ISentinelConfiguration configuration)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(bus, nameof(bus));
            Guard.AgainstNull(configuration, nameof(configuration));

            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _configuration = configuration;
            _bus = bus;
        }

        [HttpGet]
        [RequiresPermission(Permissions.View.Monitoring)]
        public IActionResult Get()
        {
            using (_databaseContextFactory.Create())
            {
                return Ok(Data(_endpointQuery.All()));
            }
        }

        [HttpGet("{search}")]
        [RequiresPermission(Permissions.View.Monitoring)]
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

        [HttpGet("statistics")]
        [RequiresPermission(Permissions.View.Monitoring)]
        public IActionResult GetStatistics()
        {
            List<Endpoint> endpoints;

            using (_databaseContextFactory.Create())
            {
                endpoints = _endpointQuery.All().ToList();
            }

            return Ok(new
            {
                Data = new
                {
                    UpCount = endpoints.Count(item =>
                        GetHeartbeatStatus(item).Equals("up", StringComparison.InvariantCultureIgnoreCase)),
                    DownCount = endpoints.Count(item =>
                        GetHeartbeatStatus(item).Equals("down", StringComparison.InvariantCultureIgnoreCase)),
                    RecoveryCount = endpoints.Count(item =>
                        GetHeartbeatStatus(item).Equals("recovery", StringComparison.InvariantCultureIgnoreCase))
                }
            });
        }

        private string GetHeartbeatStatus(Endpoint endpoint)
        {
            var now = DateTime.Now;

            var heartbeatStatus = "up";

            try
            {
                var heartbeatIntervalDuration = TimeSpan.Parse(endpoint.HeartbeatIntervalDuration);
                var heartbeatExpiryDate = now.Subtract(heartbeatIntervalDuration);

                if (endpoint.HeartbeatDate < heartbeatExpiryDate)
                {
                    heartbeatStatus = endpoint.HeartbeatDate <
                                      heartbeatExpiryDate.Subtract(_configuration.HeartbeatRecoveryDuration)
                        ? "down"
                        : "recovery";
                }
            }
            catch
            {
                heartbeatStatus = "unknown";
            }

            return heartbeatStatus;
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
                    endpoint.HeartbeatIntervalDuration,
                    endpoint.HeartbeatDate,
                    InboxWorkQueueUriSecured = GetSecuredUri(endpoint.InboxWorkQueueUri),
                    InboxDeferredQueueUriSecured = GetSecuredUri(endpoint.InboxDeferredQueueUri),
                    InboxErrorQueueUriSecured = GetSecuredUri(endpoint.InboxErrorQueueUri),
                    ControlInboxWorkQueueUriSecured = GetSecuredUri(endpoint.ControlInboxWorkQueueUri),
                    ControlInboxErrorQueueUriSecured = GetSecuredUri(endpoint.ControlInboxErrorQueueUri),
                    OutboxWorkQueueUriSecured = GetSecuredUri(endpoint.OutboxWorkQueueUri),
                    OutboxErrorQueueUriSecured = GetSecuredUri(endpoint.OutboxErrorQueueUri),
                    HeartbeatStatus = GetHeartbeatStatus(endpoint)
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

        [RequiresPermission(Permissions.Manage.Monitoring)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _bus.Send(new RemoveEndpoint
            {
                Id = id
            });

            return Ok();
        }
    }
}