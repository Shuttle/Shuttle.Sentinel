using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Shuttle.Core.Data;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel
{
	public static class WindsorContainerExtensions
	{
		public static void RegisterConfiguration(this IWindsorContainer container, ISentinelConfiguration configuration)
		{
			Guard.AgainstNull(container, "container");
			Guard.AgainstNull(configuration, "configuration");

			container.Register(Component.For<ISentinelConfiguration>().Instance(configuration));
			container.Register(Component.For<IAuthenticationService>().ImplementedBy(configuration.AuthenticationServiceType));
			container.Register(Component.For<IAuthorizationService>().ImplementedBy(configuration.AuthorizationServiceType));

			container.Resolve<IConfiguredDatabaseContextFactory>().ConfigureWith(configuration.ProviderName, configuration.ConnectionString);
		}

		public static void RegisterDataAccess(this IWindsorContainer container, string assemblyName)
		{
			Guard.AgainstNull(container, "container");

			container.RegisterDataAccess(Assembly.Load(assemblyName));
		}

		public static void RegisterDataAccess(this IWindsorContainer container, Assembly assembly)
		{
			Guard.AgainstNull(container, "container");

			container.Register(assembly, typeof (IDataRowMapper<>));

			container.Register(assembly, "Repository");
			container.Register(assembly, "Query");
			container.Register(assembly, "Factory");
		}

		public static void RegisterDataAccessCore(this IWindsorContainer container)
		{
			Guard.AgainstNull(container, "container");

			container.Register(Component.For<IDbCommandFactory>().ImplementedBy<DbCommandFactory>());
			container.Register(Component.For<IDatabaseGateway>().ImplementedBy<DatabaseGateway>());
			container.Register(Component.For<IDbConnectionFactory>().ImplementedBy<DbConnectionFactory>());
			container.Register(Component.For<IDatabaseContextFactory, IConfiguredDatabaseContextFactory>().ImplementedBy<DatabaseContextFactory>());
			container.Register(Component.For(typeof(IDataRepository<>)).ImplementedBy(typeof(DataRepository<>)));
		}

		public static void Register(this IWindsorContainer container, string assemblyName, string endsWith)
		{
			container.Register(Assembly.Load(assemblyName), endsWith);
		}

		public static void Register(this IWindsorContainer container, Assembly assembly, string endsWith)
		{
			Guard.AgainstNull(container, "container");
			Guard.AgainstNull(assembly, "assembly");
			Guard.AgainstNullOrEmptyString(endsWith, "endsWith");

			container.Register(
				Classes
					.FromAssembly(assembly)
					.Pick()
					.If(type => type.Name.EndsWith(endsWith))
					.WithServiceFirstInterface()
					.Configure(c => c.Named(c.Implementation.UnderlyingSystemType.Name)));
		}

		public static void Register(this IWindsorContainer container, string assemblyName, Type type)
		{
			container.Register(Assembly.Load(assemblyName), type);
		}

		public static void Register(this IWindsorContainer container, Assembly assembly, Type type)
		{
			Guard.AgainstNull(container, "container");
			Guard.AgainstNull(assembly, "assembly");
			Guard.AgainstNull(type, "type");

			container.Register(
				Classes
					.FromAssembly(assembly)
					.BasedOn(type)
					.WithServiceFirstInterface()
					.Configure(c => c.Named(c.Implementation.UnderlyingSystemType.Name)));
		}

		public static void Register(this IWindsorContainer container, string assemblyName, Type type,
			string endsWith)
		{
			container.Register(Assembly.Load(assemblyName), type, endsWith);
		}

		public static void Register(this IWindsorContainer container, Assembly assembly, Type type, string endsWith)
		{
			Guard.AgainstNull(container, "container");
			Guard.AgainstNull(assembly, "assembly");
			Guard.AgainstNull(type, "type");
			Guard.AgainstNullOrEmptyString(endsWith, "endsWith");

			container.Register(
				Classes
					.FromAssembly(assembly)
					.Pick()
					.If(candidate => candidate.Name.EndsWith(endsWith, StringComparison.OrdinalIgnoreCase)
					                 &&
					                 type.IsAssignableFrom(candidate))
					.LifestyleTransient()
					.WithServiceFirstInterface()
					.Configure(c => c.Named(c.Implementation.UnderlyingSystemType.Name)));
		}
	}
}