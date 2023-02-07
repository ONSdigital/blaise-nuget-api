# blaise-nuget-api

The idea behind this repository was to provide an API abstraction of Blaise API that could be produced as a NuGet package and shared across our C# repositories.

# Setup Development Environment

Clone the git repository to your IDE of choice. Visual Studio 2019 is recommended.

In order to run the behavioural tests, you will need to populate the key values in the appsettings.json file according to your local RabbitMq configuration. **Never commit App.config with key values.**

# Concepts

This solution utilizes concepts from the SOLID principles of development. In order to facilitate unit testing, dependency injection has been used throughout the code base. However, as this will be presented as a NuGet package 
  
# Use

This repository generates two NuGet packages 'Blaise.Nuget.Api' and 'Blaise.Nuget.Api.Contracts'. The Contracts package contains the interface for the API and you can use this package in your unit test projects 
mocks where you will not need to consume the complete functionality. 

You can use the fluid API style class 'FluentBlaiseApi' which implements the 'IFluentBlaiseApi' interface.
		
Copyright (c) 2021 Crown Copyright (Government Digital Service)		

# Azure Pipeline

In the root of the repository is the 'build-api.yml' file. This file contains various config and is used to create/configure an azure pipeline. This is triggered whenever changes are pushed to main by default.

To update the pipeline, make changes and then commit to this GitHub repository. If you need to test on a feature branch, commit/push changes and then go into Azure DevOps. From there you can run the pipeline manually by selecting the relevant branch. 