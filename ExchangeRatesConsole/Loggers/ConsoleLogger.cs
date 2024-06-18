namespace ExchangeRatesConsole.Loggers
{
	/// <summary>
	/// Логгирование в консоль
	/// </summary>
	public class ConsoleLogger : ILogger
	{
		public void WriteLog(string message)
		{
			Console.WriteLine(message);
		}
	}
}
