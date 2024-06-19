if db_id('ExchangeRates') is null
	create database ExchangeRates;
go
use ExchangeRates;
go
-- �������� ������� � ��������
if object_id('currencies', 'U') is null
	create table dbo.currencies
	(
		 currency_id int not null -- id ������
		,iso_num_code int not null -- �������� ��� iso
		,iso_char_code varchar(20) not null -- ��������� ��� iso
		,currency_name varchar(100) not null -- ��� ������
		,nominal int not null -- �������
		,external_id varchar(20) not null -- ������� id �� ��
		,constraint PK_currencies primary key(currency_id)
	)
go
if object_id('exchange_rates', 'U') is null
-- �������� ������� � ������� �����
	create table dbo.exchange_rates
	(
		 exchange_rate_id int not null -- id ������ ����� ������
		,exchange_rate_date date not null -- ���� ����� ������
		,rate decimal(24,2) not null -- ��������
		,rate_with_nominal decimal(24,2) not null -- �������� � ������ ��������
		,currency_id int not null -- id ������
		,constraint PK_exchange_rates primary key(exchange_rate_id)
		,constraint FK_exchange_rates_currencies foreign key(currency_id) references currencies(currency_id)
	)
go
	alter table dbo.exchange_rates alter column rate decimal(24,4) not null
	alter table dbo.exchange_rates alter column rate_with_nominal decimal(24,4) not null
go