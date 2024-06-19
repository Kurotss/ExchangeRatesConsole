sleep 10
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P p7OzUlIM -d master -i createTablesScript.sql \
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P p7OzUlIM -d master -i spGetExchangeRatesDates.sql \
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P p7OzUlIM -d master -i spInsertExchangeRate.sql