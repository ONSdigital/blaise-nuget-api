# Blaise NuGet API 

This Blaise API library offers an abstraction of the interfaces provided by the official Blaise API DLL library. It offers a high level CRUD style interface that is easier to work with. This library is available internally to the ONS via an Azure DevOps artifact feed.

# Concepts

This solution utilizes concepts from the SOLID principles of development. In order to facilitate unit testing, dependency injection has been used throughout the code base.

# Local setup

To run tests locally you will need a PAT to login to the NuGet API.

In Azure Dev Ops on your on-net:
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

In Miscrosoft Visual Studio, login to the NuGet API.  Either...
	- Build the solution and wait to be prompted for sign in...
	- Failing that, right-click Solution Explore and select Manage NuGet Packages for Solution
		- In the far-right, select CSharp from the Source drop-down and wait to be prompted for sign in...

Sign in, using your windows email address and your newly created PAT as the password
	- Failing that, ask Al for the magic windows explorer link to the config...

Populate the App.config file accordingly, **never commit a populated App.config file!**

Finally, build solution and run the tests.

# Usage

This repository contains several packages. The contracts package contains the interface for the API, you can use this package in your project unit test mocks, so you do not need to consume the complete functionality.

You can use the fluid API style class 'FluentBlaiseApi', which implements the 'IFluentBlaiseApi' interface.
		
# Azure DevOps pipeline

The yml file in the root of the repository is used to create the Azure DevOps pipeline. The pipeline is triggered whenever changes are pushed to main.

To update the pipeline, make changes to the yml file and then commit/push to this GitHub repository. If you need to test on a feature branch, commit/push changes and then go into the Azure DevOps console UI. From there you can run the pipeline manually by selecting the relevant branch. 
