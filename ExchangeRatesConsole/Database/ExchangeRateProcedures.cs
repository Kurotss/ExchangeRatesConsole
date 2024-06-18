using Dapper;
using ExchangeRates.XmlModels;
using ExchangeRatesConsole.Loggers;
using System.Data;

namespace ExchangeRatesConsole.Database
{
	/// <summary>
	/// Вызов хранимых процедур, связанных с курсами валют
	/// </summary>
	public class ExchangeRateProcedures
	{
		public ExchangeRateProcedures(ILogger logger, IDbConnection dbConnection)
		{
			_logger = logger;
			_dbConnection = dbConnection;
		}

		private readonly ILogger _logger;
		private readonly IDbConnection _dbConnection;

		/// <summary>
		/// Вставка курса валюты в БД
		/// </summary>
		public void InsertExchangeRate(Valute exchangeRate, DateTime date)
		{
			try
			{
				if (!decimal.TryParse(exchangeRate.Value, out decimal value))
					throw new ArgumentException($"Некорректное значение Value: {exchangeRate.Value}");
				if (!decimal.TryParse(exchangeRate.VunitRate, out decimal vunitRate))
					throw new ArgumentException($"Некорректное значение VunitRate: {exchangeRate.VunitRate}");

				var p = new DynamicParameters();
				p.Add("@external_id", exchangeRate.ID);
				p.Add("@iso_num_code", exchangeRate.NumCode);
				p.Add("@iso_char_code", exchangeRate.CharCode);
				p.Add("@nominal", exchangeRate.Nominal);
				p.Add("@name", exchangeRate.Name);
				p.Add("@rate", value);
				p.Add("@rate_with_nominal", vunitRate);
				p.Add("@date", date);

				_dbConnection.Execute("spInsertExchangeRate", param: p, commandType: CommandType.StoredProcedure);

				_logger.WriteLog($"{DateTime.Now}: Добавлены курсы валюты '{exchangeRate.Name}' на {date}");
			}
			catch (Exception ex)
			{
				_logger.WriteLog(ex.ToString());
			}
		}

		/// <summary>
		/// Получение списка дат, за которые есть курсы валют в БД между startDate и endDate
		/// </summary>
		public HashSet<DateTime> GetExchangeRatesDates(DateTime startDate, DateTime endDate)
		{
			try
			{
				var p = new DynamicParameters();
				p.Add("@start_date", startDate);
				p.Add("@end_date", endDate);

				HashSet<DateTime> dates = _dbConnection.Query<DateTime>("spGetExchangeRatesDates", param: p, commandType: CommandType.StoredProcedure).ToHashSet();
				return dates;
			}
			catch (Exception ex)
			{
				_logger.WriteLog(ex.ToString());
				return [];
			}
		}
	}
}
