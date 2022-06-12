USE [master]
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'UmbracoDb')
BEGIN

    CREATE DATABASE [UmbracoDb];

END;
GO

USE UmbracoDb;
