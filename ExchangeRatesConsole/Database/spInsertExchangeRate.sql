/*
Вставка строки с курсом валюты
*/
use ExchangeRates
go
if object_id('spInsertExchangeRate') IS NULL
    exec('create procedure dbo.spInsertExchangeRate as set nocount on;')
go
alter procedure dbo.spInsertExchangeRate
	 @external_id varchar(20) -- внешний id из цб
	,@iso_num_code int -- цифровой код iso
	,@iso_char_code varchar(10) -- буквенный код iso
	,@nominal int -- номинал
	,@name varchar(100) -- имя валюты
	,@rate decimal(24,4) -- значение
	,@rate_with_nominal decimal(24,4) -- значение с учётом номинала
	,@date date -- дата курса валюты
with encryption
as
begin
	begin try
		begin transaction
		--
		declare @currency_id int,
			@exchange_rate_id int,
			@date_varchar varchar(20)
		-- если нет валюты в таблице, то она добавляется для получения далее внешнего ключа
		if not exists(select 1 from currencies where external_id = @external_id)
		begin
			select @currency_id = isnull(max(currency_id), 0) + 1 from currencies
			insert currencies(currency_id, iso_num_code, iso_char_code, currency_name, nominal, external_id)
			values(@currency_id, @iso_num_code, @iso_char_code, @name, @nominal, @external_id)
		end
		else
			select @currency_id = currency_id from currencies where external_id = @external_id
		--
		if exists(select 1 from exchange_rates where currency_id = @currency_id and exchange_rate_date = @date)
		begin
			set @date_varchar = cast(@date as varchar)
			raiserror('Уже существует курс валюты %s на дату %s', 11, 1, @name, @date_varchar)
		end
		-- вставка строки с курсом валюты	
		select @exchange_rate_id = isnull(max(exchange_rate_id), 0) + 1 from exchange_rates
		insert exchange_rates(exchange_rate_id, exchange_rate_date, rate, rate_with_nominal, currency_id)
		values(@exchange_rate_id, @date, @rate, @rate_with_nominal, @currency_id)
	--
		commit transaction
	end try
	begin catch
		rollback transaction
		print error_message()
	end catch
end
go