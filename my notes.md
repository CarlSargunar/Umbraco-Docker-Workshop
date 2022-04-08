# Docker

    Wrap code and infrastructure toghether so that wherever it runs it runs the same way.


# Reasons NOT to run a database in a container

    - Security patches?
    - What happens to storage when container is takendown
    - Scaling?


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
    