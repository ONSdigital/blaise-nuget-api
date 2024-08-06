# Blaise NuGet API 

This NuGet package wraps the official Blaise NuGet package, providing a simplified and more intuitive CRUD-style interface for interacting with Blaise. This package is available internally to the ONS via an Azure DevOps artifact feed.

# Concepts

This solution is designed with maintainability and testability in mind, incorporating principles from the SOLID paradigm. To enable robust unit testing, dependency injection is consistently applied throughout the codebase, allowing for loose coupling and greater flexibility.

# Local setup

To run tests locally you will need a PAT to login to the NuGet API.

In Azure DevOps on your on-net:

- click your profile picture in the top-right
- select the ellipsis (three horiztonal dots)
- select user-settings
- select Personal access tokens
- select New token
- give it a name, any name
- add a 90 day expiration
- add all the highest permissions (read, write and manage for most of them)
- don't forget to select view the additional 29
- save your token locally
- this is your password

In Miscrosoft Visual Studio, login to the NuGet API. Either...

- Build the solution and wait for the sign-in prompt.
- Right-click Solution Explorer and select Manage NuGet Packages for Solution. In the far-right corner, select CSharp from the Source dropdown and wait for the sign-in prompt.

Sign in, using your windows email address and your newly created PAT as the password

Populate the App.config file accordingly, **never commit a populated App.config file!**

Finally, build solution and run the tests.

# Usage

This repository contains several packages. The contracts package contains the interface for the API, you can use this package in your project unit test mocks, so you do not need to consume the complete functionality.

You can use the fluid API style class 'FluentBlaiseApi', which implements the 'IFluentBlaiseApi' interface.
		
# Azure DevOps pipeline

The yml file in the root of the repository is used to create the Azure DevOps pipeline. The pipeline is triggered whenever changes are pushed to main.

To update the pipeline, make changes to the yml file and then commit/push to this GitHub repository. 

If you need to test on a feature branch, commit/push changes and then go into the Azure DevOps console UI. From there you can run the build-nuget-api pipeline manually by selecting Run pipeline, and setting the relevant branch. This will create a 'prerelease' version.  This version is annotated as {yyyy.m.dd.version}-{branch_name}, for example, 
'2023.6.22.8-BLAIS53764'. This prerelease can then be installed in other packages, such as blase-cli.  To do this, once a prerelease version has been built:
- in an IDE, go to relevant repository, such as blaise-cli
- right-click the solution and select 'Manage NuGet Packages for Solution'
- select 'CSharp' from the packages dropdown on the right
- check the 'Include prerelease' tick box
- select the relevant library as necessary, and install
-- note, on the right-hand side of the library, it will display the version it's going from, and the version it's going to, such as '2023.6.22.8' and '2023.6.22.8-BLAIS53764'

When deploying feature versions to formal environments, the above steps will need to be completed, but from a prerelease to a version. This will only need to be done for main/dev. Go into the Azure DevOps console UI, from there you can run the pipeline manually by selecting the relevant branch. 
