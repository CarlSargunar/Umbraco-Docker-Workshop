#!/bin/bash
set -e

# Function to find sqlcmd in either of the possible paths
find_sqlcmd() {
  if [ -x "/opt/mssql-tools/bin/sqlcmd" ]; then
    echo "/opt/mssql-tools/bin/sqlcmd"
  elif [ -x "/opt/mssql-tools18/bin/sqlcmd" ]; then
    echo "/opt/mssql-tools18/bin/sqlcmd"
  else
    echo "sqlcmd not found" >&2
    exit 1
  fi
}

SQLCMD=$(find_sqlcmd)

if [ "$1" = '/opt/mssql/bin/sqlservr' ]; then
  # If this is the container's first run, initialize the application database
  if [ ! -f /tmp/app-initialized ]; then
    # Initialize the application database asynchronously in a background process.
    function initialize_app_database() {
      # Wait for SQL Server to start
      sleep 15s

      # Run the setup script to create the DB and the schema in the DB
      "$SQLCMD" -C -S localhost -U sa -P P@55word!! -d master -i setup.sql

      # Note that the container has been initialized so future starts won't wipe changes to the data
      touch /tmp/app-initialized
    }
    initialize_app_database &
  fi
fi

exec "$@"
