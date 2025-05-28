# Docker CLI Quick Start

A reference guide for basic Docker CLI commands.

---

## 1. Check Docker Version

```sh
docker --version
```
Shows the installed Docker version.

---

## 2. List Docker Images

```sh
docker images
```
Lists all images downloaded on your machine.

---

## 3. List Running Containers

```sh
docker ps
```
Shows all currently running containers.

---

## 4. List All Containers (including stopped)

```sh
docker ps -a
```
Lists all containers, running or stopped.

---

## 5. Pull an Image

```sh
docker pull <image-name>
```
Downloads an image from Docker Hub (e.g., `docker pull nginx`).

---

## 6. Run a Container

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

---

## 7. Stop a Running Container

```sh
docker stop <container-name or id>
```
Gracefully stops a running container.

---

## 8. Remove a Container

```sh
docker rm <container-name or id>
```
Deletes a stopped container.

---

## 9. Remove an Image

```sh
docker rmi <image-name or id>
```
Deletes an image from your local machine.

---

## 10. View Container Logs

```sh
docker logs <container-name or id>
```
Displays the logs from a container.

---

## 11. Execute a Command in a Running Container

```sh
docker exec -it <container-name or id> <command>
```
Runs a command inside a running container (e.g., open a shell):

```sh
docker exec -it webserver /bin/bash
```

---

## 12. Prune Unused Data

```sh
docker system prune
```
Removes unused containers, networks, images, and cache.

---

For more commands, see the [official Docker CLI reference](https://docs.docker.com/engine/reference/commandline/docker/).
