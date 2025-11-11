# Blaise NuGet API

This repository contains a .NET Framework class library that wraps the official Blaise NuGet package, providing a simplified and intuitive API for interacting with Blaise environments. It abstracts the complexity of the underlying Blaise APIs and exposes a consistent, testable interface that underpins our backend services and UIs.

This library is consumed by internal applications such as our RESTful API wrapper and CLI tool to simplify interactions with Blaise.

The solution produces two NuGet packages, published to a private Azure DevOps artifact feed:

- `Blaise.Nuget.Api` – The main implementation package. Reference this in your application to access the full API.

- `Blaise.Nuget.Api.Contracts` – A lightweight package containing only the public interfaces. Reference this in your unit test projects to mock the API without bringing in its dependencies.

## Getting Started

### Prerequisites and Authentication

To develop with this repository or consume its NuGet packages, you'll need to authenticate with our private Azure DevOps artifact feed. This is because both the official Blaise NuGet package and the packages produced by this solution are hosted there.

1. **Generate a Personal Access Token (PAT):** In the Azure DevOps web UI, generate a PAT with the necessary permissions to access the artifact feed.

1. **Configure your NuGet Source:** Add the feed to your environment, either through the Visual Studio UI or by manually updating your NuGet.config file (typically found at %APPDATA%\NuGet\nuget.config).

Once the source is configured, you can install the package via the NuGet Package Manager in Visual Studio.

### Local Development and Configuration

To run the service locally, you must provide the necessary connection details for a Blaise environment. You can achieve this in two ways:

- **Populate `App.config`:** Update the `App.config` file with the required Blaise connection details.
- **Use Environment Variables:** Alternatively, you can use `setx` commands to set environment variables. This is a great way to handle sensitive data. For example: `setx ENV_BLAISE_SERVER_HOST_NAME=blah /m`.

⚠️ **Important:** Never commit `App.config` files with populated secrets or credentials to source control. To safely commit your changes without including the `App.config` file, you can use the command: `git add . ':!app.config'`.

**Connecting to a Blaise Environment:** The service needs to communicate with Blaise on two specific ports which are defined in the `App.config` file. To connect to a Blaise environment deployed on Google Cloud Platform (GCP), you can open IAP tunnels to the virtual machines.

```bash
gcloud auth login

gcloud config set project ons-blaise-v2-<env>

gcloud compute start-iap-tunnel blaise-gusty-mgmt 8031 --local-host-port=localhost:8031

gcloud compute start-iap-tunnel blaise-gusty-mgmt 8033 --local-host-port=localhost:8033
```

## Continuous Integration and Publishing

This project uses an automated build pipeline to ensure consistent code quality and package delivery. The pipeline performs the following steps:

- Building the solution.
- Running unit tests.
- Enforcing code formatting standards.
- Packing and publishing the NuGet packages.

**Package Versions**

Builds are automatically given a name based on the date and a build number (e.g., 2023.10.26.1).

**Pre-release Packages**

The pipeline will automatically build and publish a pre-release version for a feature branch only when there is a Pull Request (PR) into the main branch. Otherwise, these can be run manually via the Azure DevOps web console UI. To install these packages, you must check the "Include prerelease" box in the NuGet Package Manager.

**Coding Standards**

The project enforces a strict set of coding and formatting rules via an `.editorconfig` file, which is used by StyleCop. Builds may error or issue warnings if these standards are not followed. You can use `dotnet format` to automatically fix some formatting issues.
