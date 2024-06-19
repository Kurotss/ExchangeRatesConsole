/*
Получение списка дат, на которые есть уже курсы валют, в промежутку между @start_date и @end_date
*/
use ExchangeRates
go
if object_id('spGetExchangeRatesDates') IS NULL
    exec('create procedure dbo.spGetExchangeRatesDates as set nocount on;')
go
create procedure dbo.spGetExchangeRatesDates
	 @start_date date
	,@end_date date
with encryption as
begin
	select exchange_rate_date
	from exchange_rates
	where exchange_rate_date between @start_date and @end_date
	group by exchange_rate_date
end
go