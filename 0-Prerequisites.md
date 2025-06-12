# Prerequisites

To attend this workshop you will need the following:

- A windows, mac or linux laptop :
    - 16gb of Memory recommended - As docker uses quite a lot of memory. (Although you should still be able to complete the workshop with 8gb)
    - At least 15Gb of available space - Database containers particularly can sometimes be quite large.
    - 64-bit processor with at least 4 cores - Docker requires a 64-bit processor to run.
- A working Docker engine installation. 
    - The simplest way to do this is using [Docker Desktop](https://docs.docker.com/get-docker/).
    - For linux, if you prefer running a full docker instance, there are installations which are distro specific available [here](https://docs.docker.com/desktop/linux/install/).
- On Windows, it is recommended to have WSL2 installed. This is the recommended way to run Docker on Windows. 
        - If you are using Docker Desktop, this will be installed for you automatically as part of installing Docker Desktop.
        - If you are using a different installation of Docker, you can install WSL2 by following the instructions [here](https://docs.microsoft.com/en-us/windows/wsl/install).
- you will need to have the following installed:
    - .NET 8.0 SDK
        - It's also worth installing the .NET 9 SDK for some of the bonus content
    - Visual Studio Code or Visual Studio 2022
    - Git

This guide is primarily focused on Windows and Mac development, but will work for Linux just as well. 

## SDK

Check the version of the .NET SDK you have installed using the following command:

```bash
dotnet sdk check
```

This workshop will require version 8.0 of the SDK, preferably the latest version available. If you need to download the SDK, you can do so from [https://aka.ms/dotnet-core-download](https://aka.ms/dotnet-core-download).

It's also useful to have the .NET 9 SDK installed for some of the bonus content, but this is not required for the main workshop.

In the example below, there are updates available to the SDK, which I wull then update prior to the workshop. If you see this, please update your SDK to the latest version.

![sdk-check](/media/1_sdk_check.png)


## Test your Docker installation

It's important that your docker installation is working before you attend the workshop. We will do that by downloading and running a container image for SQL Server 2022, which will be used later in the workshop. This will also pre-cache the image on your computer so that it is available when we need it.

**Note** : If you have SQL server running on your host machine, you will need to stop it before running the container, as it will use the same port (1433) as the container. 

```bash
docker run --platform linux/amd64 --name test_sql_server_2022 -m 2g -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=SQL_PassW0rd!!' -p 14330:1433 mcr.microsoft.com/mssql/server:2022-latest
```

This command will run a SQL Server 2022 container in interactive mode, with the name `test_sql_server_2022`. It sets the memory limit to 2GB (`-m 2g`), accepts the EULA, sets the SA password, and mounts two volumes for system and user data.

It also maps port 14330 on your host to port 1433 in the container. Don't worry too much about the details of this command, we will go through it in more detail during the workshop.

It will fail if there is already a container with the name `test_sql_server_2022` running, so if you have run this command before, you will need to stop and remove the existing container first. You can do this with the following commands:

```bash
docker stop test_sql_server_2022
docker rm test_sql_server_2022
```



![Test Docker is running and you can start a SQL server](/media/Docker_Test.png)

You will see the container running in your Docker Desktop application. 

## Tools and Set-up

During this workshop it is recommended that you use [Visual Studio Code](https://code.visualstudio.com/) to run the workshop, and that you have the Auto-save feature enabled. To do this, go to the **File** menu, and select **Autosave**.

There is an extension for Visual Studio Code which can be helpful : [Containers in Visual Studio Code](https://code.visualstudio.com/docs/containers/overview).

Where commands are executed, you should be using the built in terminal from VS Code, and not the command line. To open the terminal, select **View**, then **Terminal**.

Wherever there are something for you to do I will add the flag ***Action:***. This will indicate to you that you should do the action described in the instruction.

During the workshop please take extra care to make sure you have the right path according to the instructions. There are a lot of similarly named files and folders, and you will need to be careful to not mix them up.

## Check your WSL installation

**This applies to Windows Only**

If you are using Windows, you will need to ensure that your WSL (Windows Subsystem for Linux) installation is up to date. Docker Desktop relies on WSL to run containers on Windows, and an outdated version can cause issues. To check if your WSL installation is up to date, run the following command in your terminal

```bash 
wsl --status
```

You should see output similar to the following:

```
Default Distribution: Ubuntu-20.04
Default Version: 2
```

If you need to update WSL, you can do so with the following command:

```bash 
wsl --update
```
