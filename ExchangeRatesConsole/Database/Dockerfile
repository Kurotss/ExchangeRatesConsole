#взято с https://stackoverflow.com/questions/69941444/how-to-have-docker-compose-init-a-sql-server-database
FROM mcr.microsoft.com/mssql/server:2017-latest

WORKDIR /usr/src/app

COPY createTablesScript.sql /usr/src/app
COPY Dockerfile /usr/src/app
COPY entrypoint.sh /usr/src/app
COPY run-initialization.sh /usr/src/app
COPY spGetExchangeRatesDates.sql /usr/src/app
COPY spInsertExchangeRate.sql /usr/src/app

ENV MSSQL_SA_PASSWORD=p7OzUlIM
ENV ACCEPT_EULA=N

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh