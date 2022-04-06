# Prerequisites

To attend this workshop you will need the following:

- A windows, mac or linux laptop
    - As docker uses quite a lot of memory, it is recommended to have at least 16Gb of Ram, but you will be able to complete the workshop with 8gb.
    - Enough free space to store container images and instances. Recommended to have at least 15Gb of available space. Database containers particularly can sometimes be quite large.
- A working Docker engine installation. The simplest way to do this is using [Docker Desktop](https://docs.docker.com/get-docker/) for windows and mac
    - For linux there are installations which are distro specific available [here](https://docs.docker.com/engine/install/).

## Test your Docker installation

It's important that your docker installation is working before you attend the workshop. To test that you can run the following command on your terminal.

    docker run -d -p 9080:80 docker/getting-started 

This will run the default 'getting started' container in port 9080. You can then access the container at http://localhost:9080 when it is running.

The reason this is not using the standard port 80 is that a lot of web developers on Windows will have IIS running on port 80. This will cause issues when trying to access the container.
