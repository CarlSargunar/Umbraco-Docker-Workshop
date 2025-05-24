# Prerequisites

To attend this workshop you will need the following:

- A windows, mac or linux computer with : 
    - As docker uses quite a lot of memory, it is recommended to have at least 16Gb of Ram, but you will be able to complete the workshop with 8gb.
    - Enough free space to store container images and instances. Recommended to have at least 15Gb of available space. Database containers particularly can sometimes be quite large.
- A working Docker engine installation. 
    - The simplest way to do this is using [Docker Desktop](https://docs.docker.com/get-docker/).
    - For linux, if you prefer running a full docker instance, there are installations which are distro specific available [here](https://docs.docker.com/desktop/linux/install/).

This guide is primarily focused on Windows and Mac development, but will work for Linux just as well. 

## SDK

Check the version of the .NET SDK you have installed using the following command:

    dotnet sdk check

This workshop will require version 8.0 of the SDK, preferrably the latest version available. If you need to download the SDK, you can do so from [https://dotnet.microsoft.com/en-us/download/visual-studio-sdks](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks).

![sdk-check](/media/1_sdk_check.png)


## Test your Docker installation

It's important that your docker installation is working before you attend the workshop. We will do that by downloading and running a container image for SQL Server 2022, which will be used later in the workshop. This will also pre-cache the image on your comptuer so that it is available when we need it.

```bash
docker run --platform linux/amd64 -d --name test_sql_server_2022 -m 2g -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=SQL_password123' -v dev_sql_system_22:/var/opt/mssql -v dev_sql_user_22:/var/opt/sqlserver -p 14330:1433 mcr.microsoft.com/mssql/server:2022-latest
```

This command will run a SQL Server 2022 container in detached mode (`-d`), with the name `test_sql_server_2022`. It sets the memory limit to 2GB (`-m 2g`), accepts the EULA, sets the SA password, and mounts two volumes for system and user data. It also maps port 14330 on your host to port 1433 in the container. Don't worry too much about the details of this command, we will go through it in more detail during the workshop.

![Test Docker is runnig and you can start a SQL server](/media/Docker_Test.png)

You will see the container running in your Docker Desktop application, and you can also check that it is running by using the following command:

```bash
docker ps
```
This command will list all running containers, and you should see `test_sql_server_2022` in the list.
If you see the container running, then your Docker installation is working correctly. If you encounter any issues, please refer to the [Docker documentation](https://docs.docker.com/desktop/troubleshoot-and-support/troubleshoot/) for troubleshooting tips.

