# Umbraco and Docker Workshop - Introduction

Before you start on this workshop, Fork this repository to your own GitHub account, and then clone it to your local machine. You can do this using the GitHub Desktop application or the command line.

The folders which are in this workshop are:

- **Files** - This folder contains pre-created files which will be used in this workshop to save you typing everything out manually
- **Media** - The images used in this workshop are stored in there
- **Workshop Complete** - This folder contains a fully complete version of the workshop which can be used for reference in case you run into problems, in a zipped up file. No cheating - you won't learn if you do, but it's a useful guide for reference if you get stuck 🙂 
- **Workshop** - This will be the active folder where the workshop is being run from, and all files you create and edit will be in this folder. This will be created in the next step.


## Working Folder

***Action:*** In the root of your cloned repostory, **create a folder called Workshop**. In your terminal window, change directory to the **Workshop** directory. All exercises will be completed in this folder.

Note: This folder is deliberately ignored in the .gitignore file, and will not be committed to the repository. This is to ensure that you can run the workshop multiple times without having to delete files or folders. If you want to track files as you do the workshop, you can modify the .gitignore file to remove the Workshop folder from being ignored.

Open a new terminal window in Visual Studio Code, and ensure you are in the **Workshop** folder. All the commands you run in this workshop will be run from this folder. 


## A Note on Windows vs Linux Line Endings

When you clone files onto windows the line endings may be checked out using the windows style CRLF (Carriage-Return, Line-Feed). For files which need to run on Linux based containers, you need to change this to LF (Line-Feed only). This can be done using the option as shown below.

![image](media/6_VSCodeLineEndings.png)

If it shows CRLF, click on the label and at the top you can change it to LF.

*Note : Historically windows terminates line-endings in file with a carriage return and line feed (CRLF), while Linux uses a single line feed (LF) - and if you want to learn about the history of why then check out this awesome video from Scott Hanselman : [https://www.youtube.com/watch?v=TtiBhktB4Qg](https://www.youtube.com/watch?v=TtiBhktB4Qg)*


## Troubleshooting Tips

- **Port already in use:** Make sure no other SQL Server or process is using port 1433 on your machine.
- **Container fails to start:** Check Docker Desktop for error messages. Ensure you have enough memory allocated to Docker (at least 4GB recommended).
- **Platform errors on ARM/M1/M2:** Always use `--platform=linux/amd64` when building/running SQL Server containers on ARM-based machines.
- **Line endings issues:** Ensure all scripts and Dockerfiles use LF line endings, not CRLF.
- **Cannot connect to database:** Double-check the username (`sa`), password (`SQL_PassW0rd!!`), and port (`1433`). Make sure the container is running.

## How to Reset

If you need to completely reset your workshop environment (including all files and changes in the `Workshop` folder), you can delete the entire `Workshop` folder and remove any related Docker resources.

> **Warning:** This will permanently delete all files, projects, and changes you have made in the `Workshop` folder. Make sure to back up anything you want to keep before proceeding.

1. **Delete the Workshop folder:**
   - In your file explorer or terminal, delete the entire `Workshop` folder.

2. **Remove any containers (if running):**

You can list all running containers with:

```bash
docker ps
```

You can remove any running containers with the docker rm command, with a force stop if necessary:

```bash
docker rm -f <container_name_or_id>
```

**Recreate the `Workshop` folder and restart the exercises from the beginning.**

## Need Help?

If you get stuck or have any questions, **please raise your hand and get my attention**. I'm here to help!


## Next Steps

The first task we will do is to create a database container. To do this, please continue on the [2-Simple-Umbraco-Container](2-Simple-Umbraco-Container.md) file.