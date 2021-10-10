#!/bin/bash

( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" && \
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $1 -i $2 && \
pkill sqlservr