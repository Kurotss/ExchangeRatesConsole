using Autofac;
using ExchangeRatesConsole.HangfireServer;
using ExchangeRatesConsole.Loggers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExchangeRatesConsole
{
	public class DependenciesContainer
	{
		/// <summary>
		/// Регистрация зависимостей
		/// </summary>
		public static void RegisterDependencies()
		{
			Builder.RegisterType<ConsoleLogger>().As<ILogger>().SingleInstance();
			Builder.RegisterType<SqlConnection>().As<IDbConnection>().SingleInstance();
			Builder.RegisterType<HangfireSqlServer>().As<IHangfireServer>().SingleInstance();
		}

		private static IContainer _container;

		public static IContainer Container => _container ??= Builder.Build();

		private static ContainerBuilder _builder;
		public static ContainerBuilder Builder => _builder ??= new ContainerBuilder();
	}
}
