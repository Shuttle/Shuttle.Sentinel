using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
    public class SessionService : ISessionService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISessionRepository _sessionRepository;

        public SessionService(IAuthenticationService authenticationService, IAuthorizationService authorizationService, ISessionRepository sessionRepository)
        {
            Guard.AgainstNull(authenticationService, "authenticationService");
            Guard.AgainstNull(authorizationService, "authorizationService");
            Guard.AgainstNull(sessionRepository, "sessionRepository");

            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _sessionRepository = sessionRepository;
        }

        public RegisterSessionResult Register(string username, string password)
        {
            var authenticationResult = _authenticationService.Authenticate(username, password);

            if (!authenticationResult.Authenticated)
            {
                return RegisterSessionResult.Failure();
            }

            var session = new Session(Guid.NewGuid(), username, DateTime.Now);

            foreach (var permission in _authorizationService.Permissions(username, authenticationResult.AuthenticationTag))
            {
                session.AddPermission(permission);
            }

            _sessionRepository.Save(session);

            return RegisterSessionResult.Success(session.Token, session.Permissions);
        }
    }
}