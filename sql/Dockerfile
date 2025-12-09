FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

WORKDIR /var/opt/mssql

COPY *.sql /var/opt/mssql/init.sql

USER mssql

CMD /bin/bash -c "/opt/mssql/bin/sqlservr & sleep 25 && /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -C -i /var/opt/mssql/init.sql && tail -f /dev/null"
