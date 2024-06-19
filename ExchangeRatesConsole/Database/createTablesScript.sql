if db_id('ExchangeRates') is null
	create database ExchangeRates;
go
use ExchangeRates;
go
-- создание таблицы с валютами
if object_id('currencies', 'U') is null
	create table dbo.currencies
	(
		 currency_id int not null -- id валюты
		,iso_num_code int not null -- цифровой код iso
		,iso_char_code varchar(20) not null -- буквенный код iso
		,currency_name varchar(100) not null -- имя валюты
		,nominal int not null -- номинал
		,external_id varchar(20) not null -- внешний id из цб
		,constraint PK_currencies primary key(currency_id)
	)
go
if object_id('exchange_rates', 'U') is null
-- создание таблицы с курсами валют
	create table dbo.exchange_rates
	(
		 exchange_rate_id int not null -- id строки курса валюты
		,exchange_rate_date date not null -- дата курса валюты
		,rate decimal(24,2) not null -- значение
		,rate_with_nominal decimal(24,2) not null -- значение с учётом номинала
		,currency_id int not null -- id валюты
		,constraint PK_exchange_rates primary key(exchange_rate_id)
		,constraint FK_exchange_rates_currencies foreign key(currency_id) references currencies(currency_id)
	)
go
	alter table dbo.exchange_rates alter column rate decimal(24,4) not null
	alter table dbo.exchange_rates alter column rate_with_nominal decimal(24,4) not null
go