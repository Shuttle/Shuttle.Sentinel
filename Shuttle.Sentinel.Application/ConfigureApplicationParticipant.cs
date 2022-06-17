using System;
using System.Collections.Generic;
using System.Linq;
using Shuttle.Access.Messages.v1;
using Shuttle.Access.RestClient;
using Shuttle.Core.Contract;
using Shuttle.Core.Mediator;
using ConfigureApplication = Shuttle.Sentinel.Messages.v1.ConfigureApplication;

namespace Shuttle.Sentinel.Application
{
    public class ConfigureApplicationParticipant : IParticipant<ConfigureApplication>
    {
        private readonly IAccessClient _accessClient;
        private readonly List<string> _permissions = new List<string>
        {
            "sentinel://data-stores/manage",
            "sentinel://messages/manage",
            "sentinel://monitoring/manage",
            "sentinel://queues/manage",
            "sentinel://schedules/manage",
            "sentinel://subscriptions/manage",
            "sentinel://data-stores/view",
            "sentinel://monitoring/view",
            "sentinel://queues/view",
            "sentinel://schedules/view",
            "sentinel://subscriptions/view"
        };

        public ConfigureApplicationParticipant(IAccessClient accessClient)
        {
            Guard.AgainstNull(accessClient, nameof(accessClient));

            _accessClient = accessClient;
        }

        public void ProcessMessage(IParticipantContext<ConfigureApplication> context)
        {
            Guard.AgainstNull(context, nameof(context));

            var response = _accessClient.Permissions.Get().Result;

            if (!response.IsSuccessStatusCode ||
                response.Content == null)
            {
                throw new SentinelException(Resources.GetPermissionsException, response.Error);
            }

            var permissions = response.Content.Select(item => item.Name);

            foreach (var permission in _permissions)
            {
                if (permissions.Contains(permission))
                {
                    continue;
                }

                _accessClient.Permissions.Post(new RegisterPermission
                {
                    Name = permission,
                    Status = 1
                });
            }
        }
    }
}
