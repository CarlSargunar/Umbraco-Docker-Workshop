FROM mcr.microsoft.com/azure-sql-edge:latest

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

# Copy the database files to the container
# NOTE : This is not a recommendation for production use as the database files should be stored in a persistent volume and not be a part of the image
COPY Umbraco.mdf /var/opt/sqlserver
COPY Umbraco_log.ldf /var/opt/sqlserver

ENTRYPOINT [ "/bin/bash", "startup.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]