FROM mcr.microsoft.com/mssql/server:2019-latest as build

ENV SA_PASSWORD='tmpSQLPassword1234!'
ENV ACCEPT_EULA=true

WORKDIR /tmp

COPY ./sql/ .


RUN /tmp/createTables.sh ${SA_PASSWORD} /tmp/createTables.sql

FROM mcr.microsoft.com/mssql/server:2019-latest AS release
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=''
COPY --from=build /var/opt/mssql/data /var/opt/mssql/data

COPY ./sql/updateUser.sh .

EXPOSE 1433

CMD ./updateUser.sh 'tmpSQLPassword1234!' ${SA_PASSWORD}