---
layout: default
section: Overview
title: Getting Started
permalink: /overview/getting-started/index.html
---

Out of the box Fluidity works using PetaPoco as the persistence layer, as this is what ships with Umbraco. It is possible to change this if you like, but for the sake of getting started, we’ll assume you are using this default strategy.

Start by setting up a database table for you model (you might want to populate it with some dummy data as well whilst learning). We’ll use the following as an example

````sql
CREATE TABLE [Person] (
    [Id] int IDENTITY (1,1) NOT NULL, 
    [Name] nvarchar(255) NOT NULL, 
    [JobTitle] nvarchar(255) NOT NULL, 
    [Email] nvarchar(255) NOT NULL, 
    [Telephone] nvarchar(255) NOT NULL, 
    [Avatar] nvarchar(255) NOT NULL
);
````

Then, create the associated poco model

````csharp
[TableName("Person")]
public class Person
{
    [PrimaryKeyColumn]
    public int Id { get; set; }
    public string Name { get; set; }
    public string JobTitle { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Avatar { get; set; }
}
````

With the database and model setup, we can now start to configure Fluidity itself. The entry point for a Fluidity configuration is via a class that inherits from the abstract base class `FluidityConfigModule`. This class exposes a single method, `Configure`, which is passed a `FluidityConfig` element against which you can start configuring your UI.

````csharp
public class FluidityBootstrap : FluidityConfigModule
{
    public override void Configure (FluidityConfig  config) 
    {
        // Your configuration goes here...
    }
}
````

An example of a basic configuration for our model might look something like this

[CODE SNIPPET]

Which in turn will generate a fully working user interface like this

[SCREENSHOT]

As you can see, with very little code you can start to create very powerful interfaces for your custom models. 

To see all the possible configuration options for each of the UI components, be sure to check out the [API section]({{ site.baseurl }}/api/conventions/).
