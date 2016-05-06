/*
    This file forms part of Shuttle.Sentinel.

    Shuttle.Sentinel - A management and monitoring solution for shuttle-esb implementations. 
    Copyright (C) 2016  Eben Roux

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.WebApi
{
    public class ApiResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public ApiResolver(IWindsorContainer container)
        {
            Guard.AgainstNull(container, "container");

            _container = container;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<IEnumerable<object>>();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}