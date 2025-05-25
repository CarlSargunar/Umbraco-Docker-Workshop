# Exercise 2. Creating the basic Umbraco Editor Site Container

We are going to create our Umbraco website running locally on your machine natively, and connected to the database container we just created.

## Installing Umbraco Template and start Website

***Action:*** Install the Umbraco .NET Template.

    dotnet new install Umbraco.Templates::13.8.1 --force

## 2.1 Start a container to run the database

***Action:*** Create a new Umbraco site using the following command. This will define the name of the site and the default database, as well as the default admin user and password. 

Here we will be using SQL LocalDB as the database so that in later steps it can be imported directly into the production database server. 

    dotnet new umbraco -n UmbWeb --friendly-name "Admin User" --email "admin@admin.com" --password "1234567890" --connection-string "Server=localhost;Database=UmbracoDb;User Id=sa;Password=P@55word!!;TrustServerCertificate=true"

If you are running this exercise on a Mac or Linux, you won't be able to run this site locally as it uses LocalDB, but instead will need to create your database container in step 2.2 and then run the site connecting to that image.

## 2.2 Install a template site for the exercise. 

This workshop will use the [standard starter kit for Umbraco](https://www.nuget.org/packages/Umbraco.TheStarterKit). This is a great starting point, and will let us focus on the docker integration while giving us a great site to work with, as well as a content structure which is suited to a headless API.

***Action:*** Run the following command to install the Umbraco starter kit.

    dotnet add UmbWeb package Umbraco.TheStarterKit

***Action:*** Run the website by issuing the following command. This will start the website using Kestrel, and connect to the database server in the container.

    dotnet run --project UmbWeb

This should, if there are no errors, start up the website for you to browse.

![2_run_site](media/2_run_site.png)

If you browse the site at https://localhost:11608 (or whatever port your computer reports - it may vary) you should be able to see the site running. You can also access the Umbraco backoffice at https://localhost:11608/umbraco using the credentials below.

- Username : admin@admin.com
- Password : 1234567890



