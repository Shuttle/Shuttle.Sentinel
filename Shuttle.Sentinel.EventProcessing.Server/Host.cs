﻿using System;
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
        private IEventProcessor _eventProcessor;
        private IWindsorContainer _container;
        private IEventStore _eventStore;

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
            }

            if (_eventProcessor != null)
            {
                _eventProcessor.Dispose();
            }

            if (_eventStore != null)
            {
                _eventStore.AttemptDispose();
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

            EventStore.Register(container);

            _eventStore = EventStore.Create(container);

            container.Register<UserHandler>();
            container.Register<RoleHandler>();

            _eventProcessor = container.Resolve<IEventProcessor>();

            _eventProcessor.AddProjection(new Projection("SystemUsers").AddEventHandler(container.Resolve<UserHandler>()));
            _eventProcessor.AddProjection(new Projection("SystemRoles").AddEventHandler(container.Resolve<RoleHandler>()));

            _eventProcessor.Start();
        }
    }
}