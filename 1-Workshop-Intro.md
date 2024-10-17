# Umbraco and Docker Workshop - Introduction

## Tools and Set-up

During this workshop it is recommended that you use [Visual Studio Code](https://code.visualstudio.com/) to run the workshop, and that you have the Auto-save feature enabled. To do this, go to the **File** menu, and select **Autosave**.

Where commands are executed, you should be using the built in terminal from VS Code, and not the command line. To open the terminal, select **View**, then **Terminal**.

These instructions are also available on the [GitHub repository](https://github.com/CarlSargunar/Umbraco-Docker-Workshop/blob/main/Workshop.md) or your own fork.

Wherever there are something for you to do I will add the flag ***Action:***. This will indicate to you that you should do the action described in the instruction.

During the workshop please take extra care to make sure you have the right path according to the instructions. There are a lot of similarly named files and folders, and you will need to be careful to not mix them up.


## Working Folder

***Action:*** Create a folder in the root of your application called Workshop. In your terminal window, change directory to the **Workshop** directory. All exercises will be completed in this folder.

The folders which are in this workshop are : 

- **Files** - This folder contains pre-created files which will be used in this workshop to save you typing everything out manually
- **Media** - The images used in this workshop are stored in there
- **Workshop Complete** - This folder contains a fully complete version of the workshop which can be used for reference in case you run into problems, in a zipped up file. No cheating - you won't learn if you do, but it's a useful guide for reference if you get stuck 🙂 
- **Workshop** - This will be the active folder where the workshop is being run from, and all files you create and edit will be in this folder. 


## A Note on Windows vs Linux Line Endings

When you clone files onto windows the line endings may be checked out using the windows style CRLF (Carriage-Return, Line-Feed). For files which need to run on Linux based containers, you need to change this to LF (Line-Feed only). This can be done using the option as shown below.

![image](media/6_VSCodeLineEndings.png)

If it shows CRLF, click on the label and at the top you can change it to LF.

*Note : Historically windows terminates line-endings in file with a carriage return and line feed (CRLF), while Linux uses a single line feed (LF) - and if you want to learn about the history of why then check out this awesome video from Scott Hanselman : [https://www.youtube.com/watch?v=TtiBhktB4Qg](https://www.youtube.com/watch?v=TtiBhktB4Qg)*


## Next Steps

The first task we will do is to create a database container. To do this, please continue on the [2-Database-Container.md](/2-Database-Container.md) file.
