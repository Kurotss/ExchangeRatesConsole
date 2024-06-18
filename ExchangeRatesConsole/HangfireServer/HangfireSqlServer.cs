using Autofac;
using ExchangeRatesConsole.Database;
using ExchangeRatesConsole.Loggers;
using Hangfire;
using System.Data;

namespace ExchangeRatesConsole.HangfireServer
{
	/// <summary>
	/// Hangfire сервер с хранением в Sql Server
	/// </summary>
	public class HangfireSqlServer : IHangfireServer
	{
		public HangfireSqlServer(ILogger logger, string connectionString)
		{
			_connectionString = connectionString;
			_logger = logger;
		}

		private string _connectionString;
		private ILogger _logger;

		public BackgroundJobServer? Configure()
		{
			try
			{
				GlobalConfiguration.Configuration
					.UseSqlServerStorage(_connectionString);
				var hangfireServer = new BackgroundJobServer();
				// установка ежедневного выполнения метода InsertExchangeRatesTask
				// * 21 * * * - cron-выражение, означает время 21:00 utc => 0:00 по мск
				RecurringJob.AddOrUpdate("insertExchangeRatesToday", () => InsertExchangeRatesTodayTask(_connectionString), "* 21 * * *");
				return hangfireServer;
			}
			catch (Exception ex)
			{
				_logger.WriteLog(ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// Ежедневная задача для Hangfire сервера по выгрузке курсов валют в БД
		/// </summary>
		public static void InsertExchangeRatesTodayTask(string connectionString)
		{
			using (var scope = DependenciesContainer.Container.BeginLifetimeScope())
			{
				using IDbConnection dbConnection = scope.Resolve<IDbConnection>();
				dbConnection.ConnectionString = connectionString;
				ILogger logger = scope.Resolve<ILogger>();
				Program.InsertExchangeRatesOneDay(DateTime.Today, new ExchangeRateProcedures(logger, dbConnection));
			}
		}
	}
}
