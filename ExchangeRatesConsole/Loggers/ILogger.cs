namespace ExchangeRatesConsole.Loggers
{
	/// <summary>
	/// Интерфейс для классов с логгированием
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Логгирование сообщения
		/// </summary>
		void WriteLog(string message);
	}
}
