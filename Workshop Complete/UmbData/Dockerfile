FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=SQL_password123

USER root
 
RUN mkdir /var/opt/sqlserver
 
RUN chown mssql /var/opt/sqlserver
 
ENV MSSQL_BACKUP_DIR="/var/opt/mssql"
ENV MSSQL_DATA_DIR="/var/opt/mssql/data"
ENV MSSQL_LOG_DIR="/var/opt/mssql/log"

EXPOSE 1433/tcp

# Copy Setup SQL script
COPY setup.sql /
COPY startup.sh /

ENTRYPOINT [ "/bin/bash", "startup.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]