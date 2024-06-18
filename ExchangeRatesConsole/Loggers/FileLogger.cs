using Microsoft.Extensions.Configuration;

namespace ExchangeRatesConsole.Loggers
{
	public class FileLogger : ILogger
	{
		public void WriteLog(string message)
		{
			string logFilePath = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.Build()
				.GetValue(typeof(string), "LogFilePath").ToString();
			using (StreamWriter fileStream = new StreamWriter(logFilePath, true))
			{
				fileStream.WriteLine(message);
			}
		}
	}
}
