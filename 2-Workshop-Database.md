
# Exercise 1 - Create a Database Container

The first task we will do is to create a database container, using Sql Server 2022. This will be used to store the Umbraco database, and will be used by the Umbraco website container we will create later.


## 1.1 Create a container for the database server

***Action:*** 
- Create a new folder in your copy of the repostory called **Workshop**.
- Ensure your Visual Studio Code terminal is in the new **Workshop** folder.
- In your Workshop folder, create a new folder called **UmbData**. 
- In that folder, create a blank file in the UmbData folder called **Dockerfile**. 

This will folder and the associated Dockerfile will define the database container, the image to use, and the ports it exposes and also describe the configuration we will use with that database container. 

*Note : the casing of the file is important - it needs to be called Dockerfile with no extension*

***Action:*** Paste the contents below in that file, and make sure the line endings are **LF**.

```dockerfile
FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=P@55word!!

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
```


This file will instruct Docker to create a SQL server running SQL Server 2022, will accept the End User License Agreement, and will define environmental variables to configure the paths to be used for databases. It will also configure the ports to be exposed (1433), and copy two scripts into the container. These scripts will be used to restore the database export from the container. 

*Note : We are use Azure SQL Edge here as a database container for compatibility - SQL Edge will with on both x64 as well as ARM cpus which come on Macbooks with an M1 chip.*

***Action:*** : Copy the database setup scripts and databases

- From **/Files/UmbData/setup.sql** to **/Working/UmbData/setup.sql**
- From **/Files/UmbData/startup.sh** to **/Working/UmbData/startup.sh**

These two script files will be used to create a new database if none already exists when the database container starts. That way when the website starts it will already have a database ready to use, but if the database already exists it won't restore it.

***Action:*** Once all these files exist in the Working/UmbData folder, make sure the **Dockerfile, setup.sql and startup.sh** have the correct line-endings, that they are terminated with Line Feed (LF) and NOT Carriage Return Line Feed (CRLF) (See [1-Workshop-Intro](1-Workshop-Intro.md) for details).

## 1.2 Build the database image and run the database container

All our files are ready to build the database image and run the database container, so that's the next step.

*Note : If you are running a local SQL Server on your machine, or any other process listening on port 1433, you will need to stop that process before you can run the database container, or the container will not be able to start.*

*Note : You need to ensure that you don't have a running SQL server on your host machine, as the port used will clash with your container. Stop any servers to ensure port 1433 is not in use.*

***Action:*** 

Ensure you are in the **/Working** folder in your terminal window and build the database image with the following command:

```bash
docker build --tag=umbdata ./UmbData    
```

Once the image is built, run it with the following command. 

```bash
docker run --name umbdata -p 1433:1433 --volume sqlFiles:/var/opt/mssql  -d umbdata
```

This should give you a container ID back if the container was started successfully. You should also be able to see the container running in Docker Desktop.

![Docker desktop with the datbase container running.](media/1_1_database-container.png)


## 1.3 Creating the network for our containers

Before we create website containers, we need to create a network to allow our containers to communicate. We will be using a [User Defined Bridge Network](https://docs.docker.com/network/bridge/) to let our containers communicate using container names. Without this, they would only be able to communicate with IP address. 

***Action:*** Run the following command in the terminal window to create a new Bridge network for our containers to use. 

    docker network create -d bridge umbNet    

We then need to run the database and website containers attached to this network. Since the database container is already running, we can issue the following command to attach the container to the network.

    docker network connect umbNet umbdata

You can inspect the network by running the following command.

    docker network inspect umbNet

In the output you should see the umbdata container listed in the containers section.

![Docker network inspect showing the umbdata container attached to the network.](media/docker-network.png)

### Connecting to the database container

To test that your container is running Ok, you may want to test connecting to the server. You can connect with any number of tools, e.g.Sql Server Management Studio, LinqPad, or the Visual Studio Code SQL Server extension using the following credentials : 

- Host : Localhost
- Username : sa
- Password : P@55word!!
- Port : 1433

## Next Steps

The first task we will do is to create the umbraco website container. To do this, please open the [3-Workshop-Website.md](/3-Workshop-Website.md) file.
