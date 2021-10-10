#!/bin/bash

CMD=$(echo "ALTER LOGIN sa WITH PASSWORD = '"$2"';")
( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started"
echo "updating password"
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $1 -Q "$CMD"
echo "done."
pkill sqlservr && sleep 10
/opt/mssql/bin/sqlservr 