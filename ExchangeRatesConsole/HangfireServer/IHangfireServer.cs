using Hangfire;

namespace ExchangeRatesConsole.HangfireServer
{
	/// <summary>
	/// Интерфейс для классов Hangfire сервера
	/// </summary>
	public interface IHangfireServer
	{
		/// <summary>
		/// Настройка Hangfire сервера
		/// </summary>
		BackgroundJobServer? Configure();
	}
}
