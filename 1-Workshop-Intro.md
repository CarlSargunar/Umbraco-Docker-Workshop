# Umbraco and Docker Workshop - Introduction

Before you start on this workshop, Fork this repository to your own GitHub account, and then clone it to your local machine. You can do this using the GitHub Desktop application or the command line.

The folders which are in this workshop are:

- **Files** - This folder contains pre-created files which will be used in this workshop to save you typing everything out manually
- **Media** - The images used in this workshop are stored in there
- **Workshop Complete** - This folder contains a fully complete version of the workshop which can be used for reference in case you run into problems, in a zipped up file. No cheating - you won't learn if you do, but it's a useful guide for reference if you get stuck 🙂 
- **Workshop** - This will be the active folder where the workshop is being run from, and all files you create and edit will be in this folder. This will be created in the next step.


## Working Folder

***Action:*** In the root of your cloned repostory, **create a folder called Workshop**. In your terminal window, change directory to the **Workshop** directory. All exercises will be completed in this folder.

Note: This folder is deliberately ignored in the .gitignore file, and will not be committed to the repository. This is to ensure that you can run the workshop multiple times without having to delete files or folders.


## A Note on Windows vs Linux Line Endings

When you clone files onto windows the line endings may be checked out using the windows style CRLF (Carriage-Return, Line-Feed). For files which need to run on Linux based containers, you need to change this to LF (Line-Feed only). This can be done using the option as shown below.

![image](media/6_VSCodeLineEndings.png)

If it shows CRLF, click on the label and at the top you can change it to LF.

*Note : Historically windows terminates line-endings in file with a carriage return and line feed (CRLF), while Linux uses a single line feed (LF) - and if you want to learn about the history of why then check out this awesome video from Scott Hanselman : [https://www.youtube.com/watch?v=TtiBhktB4Qg](https://www.youtube.com/watch?v=TtiBhktB4Qg)*


## Next Steps

The first task we will do is to create a database container. To do this, please continue on the [2-Simple-Umbraco-Container](2-Simple-Umbraco-Container.md) file.
