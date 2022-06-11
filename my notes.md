# Docker

    Wrap code and infrastructure toghether so that wherever it runs it runs the same way.

# Reasons NOT to run a database in a container

    - Security patches?
    - What happens to storage when container is takendown
    - Scaling?
    - Better option - use PaS databases, SQL azure for example



# Environmental variables 
    - https://our.umbraco.com/packages/developer-tools/cultivenvironmentinspect/
        - Show environmetal variables for all environments. Useful in containers

# Deployment

https://www.youtube.com/watch?v=xTwM-g40vd0

    - Octopus deploy
        - Runbooks :
            - Scripts/process used around the code 
            - https://docs.octopus.com/display/OD/Runbooks

    - AWS
    - Fargate
    - Cloud formation
        - Infrastructure as code
    
# v10

- https://umbraco.com/blog/umbraco-10-release-candidate/
    - https://umbraco.com/blog/umbraco-product-update-may-11-2022/

$ dotnet nuget add source https://myget.org/F/umbraconightly/api/v3/index.json --name umbraconightly
$ dotnet new -i Umbraco.Templates::10.0.0-rc4
$ dotnet new umbraco --name {{project name}}

https://twitter.com/BjarkeBerg/status/1502705208203358209
