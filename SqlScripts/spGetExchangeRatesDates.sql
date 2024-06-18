/*
��������� ������ ���, �� ������� ���� ��� ����� �����, � ���������� ����� @start_date � @end_date
*/
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