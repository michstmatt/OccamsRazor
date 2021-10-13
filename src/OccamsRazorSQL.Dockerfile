FROM mariadb:latest as build

ENV MARIADB_ROOT_PASSWORD='tmpSQLPassword1234!'

WORKDIR /tmp

COPY ./sql/createMariaSchema.sql /docker-entrypoint-initdb.d