# Umbraco and Docker

## Prerequisites

In order to run this application you will need the following installed on your machine.

- Visual Studio Code
    - There's a useful docker extension for Visual studio code : [https://code.visualstudio.com/docs/containers/overview](https://code.visualstudio.com/docs/containers/overview)
- Docker Desktop 
    - Windows subsystem for Linux (only required in Windows obviously)	
- .NET SDK version 6


## Installing Umbraco Template

Run the following to install the template

    dotnet new -i Umbraco.Templates::10.0.0-rc1
 
## Cleanup

If you want to remove the RC1 template and revert to the older version

    dotnet new -u Umbraco.Templates



