using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shuttle.Access.Mvc;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.Sentinel.DataAccess;
using Shuttle.Sentinel.DataAccess.Query;
using Shuttle.Sentinel.Messages.v1;

namespace Shuttle.Sentinel.WebApi.v1
{
    [Route("[controller]", Order = 1)]
    [Route("v{version:apiVersion}/[controller]", Order = 2)]
    [ApiVersion("1")]
    public class EndpointsController : Controller
    {
        private readonly IServiceBus _serviceBus;
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IEndpointQuery _endpointQuery;
        private readonly WebApiOptions _webApiOptions;

        public EndpointsController(IOptions<WebApiOptions> webApiOptions, IServiceBus serviceBus, IDatabaseContextFactory databaseContextFactory,
            IEndpointQuery endpointQuery)
        {
            Guard.AgainstNull(webApiOptions, nameof(webApiOptions));
            Guard.AgainstNull(webApiOptions.Value, nameof(webApiOptions.Value));
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(endpointQuery, nameof(endpointQuery));
            Guard.AgainstNull(serviceBus, nameof(serviceBus));

            _webApiOptions = webApiOptions.Value;
            _databaseContextFactory = databaseContextFactory;
            _endpointQuery = endpointQuery;
            _serviceBus = serviceBus;
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
                                      heartbeatExpiryDate.Subtract(_webApiOptions.HeartbeatRecoveryDuration)
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
                return new Uri(uri).ToString();
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
            _serviceBus.Send(new RemoveEndpoint
            {
                Id = id
            });

            return Ok();
        }
    }
}