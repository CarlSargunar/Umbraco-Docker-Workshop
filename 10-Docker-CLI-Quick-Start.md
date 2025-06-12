# Docker CLI Quick Start

A reference guide for basic Docker CLI commands.

---

## 1. Docker Info & Version

```sh
docker --version
```
Shows the installed Docker version.

```sh
docker info
```
Displays detailed information about Docker installation and resources.

---

## 2. Working with Images

### List Images

```sh
docker images
```
Lists all images downloaded on your machine.

### Pull an Image

```sh
docker pull <image-name>
```
Downloads an image from Docker Hub (e.g., `docker pull nginx`).

### Remove an Image

```sh
docker rmi <image-name or id>
```
Deletes an image from your local machine.

---

## 3. Working with Containers

### List Running Containers

```sh
docker ps
```
Shows all currently running containers.

### List All Containers (including stopped)

```sh
docker ps -a
```
Lists all containers, running or stopped.

### Run a Container

```sh
docker run <options> <image-name>
```
Creates and starts a new container from an image. Common options:
- `-d`: Detached mode (runs in background)
- `-p`: Publish a container's port to the host (e.g., `-p 8080:80`)
- `--name`: Assign a name to the container

Example:
```sh
docker run -d -p 8080:80 --name webserver nginx
```

### Stop a Running Container

```sh
docker stop <container-name or id>
```
Gracefully stops a running container.

### Start a Stopped Container

```sh
docker start <container-name or id>
```
Starts a previously stopped container.

### Restart a Container

```sh
docker restart <container-name or id>
```
Restarts a running or stopped container.

### Remove a Container

```sh
docker rm <container-name or id>
```
Deletes a stopped container.

### View Container Logs

```sh
docker logs <container-name or id>
```
Displays the logs from a container.

### Execute a Command in a Running Container

```sh
docker exec -it <container-name or id> <command>
```
Runs a command inside a running container (e.g., open a shell):

```sh
docker exec -it webserver /bin/bash
```

---

## 4. Working with Networks

### List Networks

```sh
docker network ls
```
Lists all Docker networks.

### Inspect a Network

```sh
docker network inspect <network-name>
```
Shows detailed information about a specific network.

### Create a Network

```sh
docker network create <network-name>
```
Creates a new Docker network.

### Remove a Network

```sh
docker network rm <network-name>
```
Deletes a Docker network.

---

## 5. Working with Volumes

### List Volumes

```sh
docker volume ls
```
Lists all Docker volumes.

### Inspect a Volume

```sh
docker volume inspect <volume-name>
```
Shows detailed information about a specific volume.

### Create a Volume

```sh
docker volume create <volume-name>
```
Creates a new Docker volume.

### Remove a Volume

```sh
docker volume rm <volume-name>
```
Deletes a Docker volume.

---

## 6. System Cleanup

### Prune Unused Data

```sh
docker system prune
```
Removes unused containers, networks, images, and cache.

---

For more commands, see the [official Docker CLI reference](https://docs.docker.com/engine/reference/commandline/docker/).
