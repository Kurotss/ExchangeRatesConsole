using Autofac;
using Autofac.Core;
using ExchangeRates.XmlModels;
using ExchangeRatesConsole.Database;
using ExchangeRatesConsole.HangfireServer;
using ExchangeRatesConsole.Loggers;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRatesConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
			DependenciesContainer.RegisterDependencies();

			// получение объектов из DI контейнера и получение курсов за последние 30 дней
			IHangfireServer hangfireServer;
			using (var scope = DependenciesContainer.Container.BeginLifetimeScope())
			{
				using IDbConnection dbConnection = scope.Resolve<IDbConnection>();
				dbConnection.ConnectionString = GetConnectionString();
				ILogger logger = scope.Resolve<ILogger>();
				InsertExchangeRatesLastMonth(DateTime.Today, new ExchangeRateProcedures(logger, dbConnection));

				List<Parameter> parameters = [new TypedParameter(typeof(string), GetConnectionString()), new TypedParameter(typeof(ILogger), logger)];
				hangfireServer = scope.Resolve<IHangfireServer>(parameters);
			}

			// настройка Hangfire сервера и уставнока ежедневной задачи по выгрузке
			using BackgroundJobServer backgroundJobServer = hangfireServer.Configure();
			Console.ReadLine();
		}

		public static string GetConnectionString() => new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.Build()
				.GetConnectionString("SqlServerConnection");

		/// <summary>
		/// Получение и вставка в БД курсов валют за последний месяц
		/// </summary>
		public static void InsertExchangeRatesLastMonth(DateTime date, ExchangeRateProcedures exchangeRateProcedures)
		{
			var startDate = date.AddDays(-30);
			var endDate = date.AddDays(-1);
			HashSet<DateTime> dates = exchangeRateProcedures.GetExchangeRatesDates(startDate, endDate);

			for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
			{
				if (!dates.Contains(currentDate))
					InsertExchangeRatesOneDay(currentDate, exchangeRateProcedures);
			}
		}

		/// <summary>
		/// Получение и вставка в БД курсов валют за день
		/// </summary>
		public static void InsertExchangeRatesOneDay(DateTime date, ExchangeRateProcedures exchangeRateProcedures)
		{
			string xmlExchangeRates = GetXmlExchangeRates(date);
			ValCurs exchangeRates = DeserializeXmlString<ValCurs>(xmlExchangeRates);

			foreach (var exchangeRate in exchangeRates.Valute)
			{
				exchangeRateProcedures.InsertExchangeRate(exchangeRate, date);
			}
		}

		/// <summary>
		/// Получение курсов вавлют в формате xml за день
		/// </summary>
		public static string GetXmlExchangeRates(DateTime date)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			HttpClient httpClient = new();
			HttpRequestHeaders requestHeaders = httpClient.DefaultRequestHeaders;
			requestHeaders.Add("Accept", "application/xml");

			HttpResponseMessage mes = httpClient.GetAsync($"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date:dd/MM/yyyy}").Result;

			HttpContent responseContent = mes.Content;
			string responseData = responseContent.ReadAsStringAsync().Result;

			return responseData;
		}

		/// <summary>
		/// Десериализация в объект из xml строки
		/// </summary>
		public static T DeserializeXmlString<T>(string xmlData) where T : class
		{
			XmlSerializer xmlSerializer = new(typeof(T));
			T deserializedObject;
			using (TextReader reader = new StringReader(xmlData))
			{
				deserializedObject = xmlSerializer.Deserialize(reader) as T;
			}
			return deserializedObject;
		}
	}
}
