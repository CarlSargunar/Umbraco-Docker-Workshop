FROM mcr.microsoft.com/azure-sql-edge:1.0.4

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=SQL_password123

USER root
 
ENV MSSQL_BACKUP_DIR="/var/opt/mssql"
ENV MSSQL_DATA_DIR="/var/opt/mssql/data"
ENV MSSQL_LOG_DIR="/var/opt/mssql/log"

EXPOSE 1433/tcp

COPY setup.sql /
COPY startup.sh /

ENTRYPOINT [ "/bin/bash", "startup.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]