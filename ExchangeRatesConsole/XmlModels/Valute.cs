namespace ExchangeRates.XmlModels
{
	/// <summary>
	/// Список курсов валют для десериализации
	/// </summary>
	[Serializable]
	public class ValCurs
	{
		/// <summary>
		/// Массив с курсами валют
		/// </summary>
		[System.Xml.Serialization.XmlElement("Valute")]
		public Valute[] Valute { get; set; }

		/// <summary>
		/// Дата курса
		/// </summary>
		[System.Xml.Serialization.XmlAttribute()]
		public string Date { get; set; }

		/// <summary>
		/// Имя документа
		/// </summary>
		[System.Xml.Serialization.XmlAttribute()]
		public string name { get; set; }
	}

	/// <summary>
	/// Курс валюты для десериализации
	/// </summary>
	[Serializable]
	public class Valute
	{
		/// <summary>
		/// Внешний ID курса валюты
		/// </summary>
		[System.Xml.Serialization.XmlAttribute()]
		public string ID { get; set; }

		/// <summary>
		/// Цифровой код iso
		/// </summary>
		public int NumCode { get; set; }

		/// <summary>
		/// Буквенный код iso
		/// </summary>
		public string CharCode { get; set; }

		/// <summary>
		/// Номинал
		/// </summary>
		public int Nominal { get; set; }

		/// <summary>
		/// Название валюты
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Значение курса
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Значение курса с учётом номинала
		/// </summary>
		public string VunitRate { get; set; }
	}
}
