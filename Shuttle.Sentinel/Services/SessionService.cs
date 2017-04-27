using System;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
    public class SessionService : ISessionService
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISessionRepository _sessionRepository;

        public SessionService(IDatabaseContextFactory databaseContextFactory, IAuthenticationService authenticationService, IAuthorizationService authorizationService, ISessionRepository sessionRepository)
        {
            Guard.AgainstNull(databaseContextFactory, "databaseContextFactory");
            Guard.AgainstNull(authenticationService, "authenticationService");
            Guard.AgainstNull(authorizationService, "authorizationService");
            Guard.AgainstNull(sessionRepository, "sessionRepository");

            _databaseContextFactory = databaseContextFactory;
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
            _sessionRepository = sessionRepository;
        }

        public RegisterSessionResult Register(string username, string password, Guid token)
        {
            AuthenticationResult authenticationResult;

            if (string.IsNullOrEmpty(username) || (string.IsNullOrEmpty(password) && token.Equals(Guid.Empty)))
            {
                return RegisterSessionResult.Failure();
            }

            Session session;

            if (!string.IsNullOrEmpty(password))
            {
                authenticationResult = _authenticationService.Authenticate(username, password);

                if (!authenticationResult.Authenticated)
                {
                    return RegisterSessionResult.Failure();
                }

                session = new Session(Guid.NewGuid(), username, DateTime.Now);

                foreach (var permission in _authorizationService.Permissions(username, authenticationResult.AuthenticationTag))
                {
                    session.AddPermission(permission);
                }

                using (_databaseContextFactory.Create())
                {
                    _sessionRepository.Save(session);
                }
            }
            else
            {
                using (_databaseContextFactory.Create())
                {
                    session = _sessionRepository.Get(token);

                    if (session == null)
                    {
                        return RegisterSessionResult.Failure();
                    }

                    session.Renew();

                    _sessionRepository.Renewed(session);
                }
            }

            return RegisterSessionResult.Success(session.Token, session.Permissions);
        }
    }
}