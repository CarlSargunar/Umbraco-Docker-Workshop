## Class Library

Start a new class library project.

    dotnet new classlib -n UmbLib
    dotnet sln add UmbLib
    dotnet add UmbLib package Umbraco.Cms.Core
    dotnet add UmbLib package Umbraco.Cms.Infrastructure


## Add a reference

To the Umbraco Project

    dotnet add .\UmbDock\ reference .\UmbLib\

And the Blazor

    dotnet add .\UmBlazor\ reference .\UmbLib\
