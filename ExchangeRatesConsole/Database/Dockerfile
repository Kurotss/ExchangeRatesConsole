#взято с https://stackoverflow.com/questions/46888045/docker-mssql-server-linux-how-to-launch-sql-file-during-build-from-dockerfi
FROM mcr.microsoft.com/mssql/server:2017-latest

RUN mkdir -p /opt/mssql-scripts

COPY createTablesScript.sql /opt/mssql-scripts
COPY spGetExchangeRatesDates.sql /opt/mssql-scripts
COPY spInsertExchangeRate.sql /opt/mssql-scripts

ENV MSSQL_SA_PASSWORD=p7OzUlIM
ENV ACCEPT_EULA=N

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) | sleep 20 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'p7OzUlIM' -i /opt/mssql-scripts/createTablesScript.sql \
    && pkill sqlservr 

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) & sleep 10 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'p7OzUlIM' -i /opt/mssql-scripts/spGetExchangeRatesDates.sql \
    && pkill sqlservr 

RUN ( /opt/mssql/bin/sqlservr --accept-eula & ) & sleep 10 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'p7OzUlIM' -i /opt/mssql-scripts/spInsertExchangeRate.sql \
    && pkill sqlservr 