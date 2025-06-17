# Exercise 3 - Create a Database Container

The first task we will do is to create a database container, using Sql Server 2022. This will be used to store the Umbraco database, and will be used by the Umbraco website container we will create later. 

In production 


## 3.1 Create a container for the database server

***Action:*** 
- Ensure your Visual Studio Code terminal is in the new **Workshop** folder.
- In your Workshop folder, create a new folder called **UmbData**. 
- In that folder, create a blank file in the UmbData folder called **Dockerfile**. 

This will folder and the associated Dockerfile will define the database container, the image to use, and the ports it exposes and also describe the configuration we will use with that database container. 

*Note : the casing of the file is important - it needs to be called Dockerfile with no extension*

***Action:*** Paste the contents below in that file, and make sure the line endings are **LF**.

```dockerfile
FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=SQL_PassW0rd@1234

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

Dockerfiles do not specify the platform they are built for, so by default they will be built for the platform of the machine you are running on. If you are running on an ARM64 machine (e.g. Mac M1 or M2), you will need to specify the platform when building the image, as we will do later.

***Action:*** : Copy the database setup scripts and databases.

- From **/Files/UmbData/setup.sql** to **/Workshop/UmbData/setup.sql**
- From **/Files/UmbData/startup.sh** to **/Workshop/UmbData/startup.sh**

These two script files will be used to create a new database if none already exists when the database container starts. That way when the website starts it will already have a database ready to use, but if the database already exists it won't restore it.

***Action:*** Once all these files exist in the Workshop/UmbData folder, make sure the **Dockerfile, setup.sql and startup.sh** have the correct line-endings, that they are terminated with Line Feed (LF) and NOT Carriage Return Line Feed (CRLF).

## 3.2 Build the database image and run the database container

All our files are ready to build the database image and run the database container, so that's the next step.

*Note : If you are running a local SQL Server on your machine, or any other process listening on port 1433, you will need to stop that process before you can run the database container, or the container will not be able to start.*

***Action:*** 

Ensure you are in the **/Workshop** folder in your terminal window and build the database image with the following command:

Note: The `--platform` option tells Docker to build the image for the specified platform. If you are running on an ARM64 machine (e.g. Mac M1 or M2), you will need to specify `linux/amd64` to ensure compatibility with the SQL Server image, but if you are running on an x86 machine, you can omit this option. Docker Desktop will use emulation if you are on an ARM64 machine.

```bash
# Build the database image
docker build -t umbdata:1.0.0 -t umbdata:latest ./UmbData --platform=linux/amd64
```

Once the image is built, run it with the following command.

```bash
docker run --name umbdata -p 1433:1433 --volume umbSqlFiles:/var/opt/mssql --platform=linux/amd64 -d umbdata:latest
```

Ignore the warning about the platform if you are on an ARM64 based CPU, as this image is not available on ARM64.

This should give you a container ID back if the container was started successfully. You should also be able to see the container running in Docker Desktop.

![Docker desktop with the datbase container running.](media/1_1_database-container.png)



## Understanding Docker Volumes

When running the database container, we used the `--volume umbSqlFiles:/var/opt/mssql` option. This creates a **named volume** called `umbSqlFiles` and mounts it to the `/var/opt/mssql` directory inside the container.

**What does this mean?**

- A Docker volume is a persistent storage mechanism managed by Docker.
- Data written to `/var/opt/mssql` inside the container (where SQL Server stores its databases) is actually stored in the `umbSqlFiles` volume on your host machine.
- This means your database data will **persist** even if you stop, remove, or recreate the container.
- If you delete the volume (`docker volume rm umbSqlFiles`), all data in the database will be lost.

**Why use volumes?**

- Volumes allow you to keep any data that is likely to change, and whose changes need to be persistant outside the scope of the container image.
- You can easily back up, restore, or share data between containers using volumes or bind mounts.
- A bind mount links a specific directory on your host machine to a directory in the container, but volumes are managed by Docker which takes care of paths and permissions for you, and are generally more portable and easier to manage.
    - Bind mounts are useful for development scenarios where you want to edit files and build artifacts on your host machine and see changes reflected in the container immediately
    - Bind mounts aren't explored in this workshop, but you can read more about them in the [Docker documentation](https://docs.docker.com/engine/storage/bind-mounts/).

## 3.3 Connect to the Running SQL Server

Now that your SQL Server container is running, let's connect to it to verify that everything is working correctly.

You can use any SQL client tool you prefer, such as:

- **SQL Server Management Studio (SSMS)**
- **Azure Data Studio**
- **Visual Studio Code** with the SQL Server extension
- **LinqPad**
- Or any other SQL client that supports SQL Server

**Connection details:**
- **Server/Host:** `localhost`
- **Port:** `1433`
- **Username:** `sa`
- **Password:** `SQL_PassW0rd@1234`

> If you are using Visual Studio Code, you can install the "SQL Server (mssql)" extension and use the above credentials to connect.

Once connected, you should be able to see the SQL Server instance and any databases that are present. This confirms that your containerized SQL Server is running and accessible.

At this point you can test the persistence of the databases by adding some data, or creating a new database or table. If you delete and recreate the container, the data will still be there as long as you are using the same named volume (`umbSqlFiles`).

---

## 3.4 Creating the network for our containers

Before we create website containers, we need to create a network to allow our containers to communicate. We will be using a [User Defined Bridge Network](https://docs.docker.com/network/bridge/) to let our containers communicate using container names. Without this, they would only be able to communicate with IP address.

***Action:*** Run the following command in the terminal window to create a new Bridge network for our containers to use.

```bash
docker network create -d bridge umbNet    
```

We then need to run the database and website containers attached to this network. Since the database container is already running, we can issue the following command to attach the container to the network.

```bash
docker network connect umbNet umbdata
```

You can inspect the network by running the following command.

```bash
docker network inspect umbNet
```

In the output you should see the umbdata container listed in the containers section.

![Docker network inspect showing the umbdata container attached to the network.](media/docker-network.png).

We will add subsequent containers to this network as we create them, so they can communicate with each other using their container names.

## Next Steps

The first task we will do is to create the umbraco website container. To do this, please open the [3-Workshop-Website.md](/3-Workshop-Website.md) file.

