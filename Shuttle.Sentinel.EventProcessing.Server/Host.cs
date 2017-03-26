using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using Shuttle.Core.Castle;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.Core.Log4Net;
using Shuttle.Recall;

namespace Shuttle.Sentinel.EventProcessing.Server
{
    public class Host : IHost, IDisposable
    {
        private IWindsorContainer _container;

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        public void Start()
        {
            Log.Assign(new Log4NetLog(LogManager.GetLogger(typeof(Host))));

            _container = new WindsorContainer();

            _container.RegisterDataAccessCore();
            _container.RegisterDataAccess("Shuttle.Sentinel");
            _container.RegisterDataAccess("Shuttle.Recall.Sql");

            var container = new WindsorComponentContainer(_container);

            //container.Register<IDatabaseContextCache, ThreadStaticDatabaseContextCache>();

            //container.Register<IScriptProviderConfiguration, ScriptProviderConfiguration>();
            //container.Register<IScriptProvider, ScriptProvider>();

            //container.Register<IProjectionRepository, ProjectionRepository>();
            //container.Register<IProjectionQueryFactory, ProjectionQueryFactory>();

            //container.Register<IProjectionConfiguration>(ProjectionSection.Configuration());

            EventStore.Register(container);

            _container.Register(Component.For<object>().ImplementedBy<UserHandler>().Named("UserHandler"));
            _container.Register(Component.For<object>().ImplementedBy<RoleHandler>().Named("RoleHandler"));
        }
    }
}