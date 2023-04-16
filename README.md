# AutoTrust

<p align="center">
  <img src="./Images/AutoTrust.png" alt="Auto Trust" width="250px"/>
</p>

## **Auto**matic software dependency auditing using **trust** criteria

AutoTrust is a Command Line Interface (CLI) tool for C# that fetches metadata about a NuGet package to help developers assess the package before installing it.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

## Prerequisites

- .NET 7.0 or higher

## Installing

```PowerShell
git clone https://github.com/HallvardMM/AutoTrust.git
cd AutoTrust/AutoTrust
dotnet pack
```

### Windows

```PowerShell
dotnet tool install --global --add-source ./nupkg AutoTrust
```

### Mac or Linux

```Bash
dotnet tool install --tool-path ~/bin --add-source ./nupkg AutoTrust
```

## Uninstalling

### Windows

```PowerShell
dotnet tool uninstall --global AutoTrust
```

### Mac or Linux

```Bash
dotnet tool uninstall --tool-path ~/bin AutoTrust
```

## Adding Github API

The Github api has a rate limit that can lead to unsuccessful api calls.
You can increase the limit by creating and adding a Github token.
Recommend using [fine grained personal access token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token#creating-a-fine-grained-personal-access-token) with permission for "Public Repositories (read-only)".

### Windows

The application looks for GITHUB_API_TOKEN in the process, user and machine environment variables.
It can be added to either.
Example on how to add (Insert your token instead of "github_pat_tokenString"):

```PowerShell
setx GITHUB_API_TOKEN github_pat_tokenString
```

### Mac or Linux

For MacOS or Linux and it will try to fetch from environment variables defined in the shell.
Example on how to add (Insert your token instead of "github_pat_tokenString"):

```bash
export GITHUB_API_TOKEN=github_pat_tokenString
```
NOTE: When running the "export token"-command the token will only work for the current session and not after you have closed the terminal. If you want it to work  permanently you open the terminal you will have to store the token. See below for instructions:

#### Zsh and Bash token storage 
How to permanantly store the ```GITHUB_API_TOKEN``` will depend on which shell you use. If you use zsh (mac standard) store it in ```.zshenv``` and for bash (standard for most linux distributions) store it in ```.bashrc```. To open the file ```.zshenv``` in the terminal run ```$ vim ~/.zshenv``` or to open in text editor run ```$ touch ~/.zshenv; open ~/.zshenv```.

For both ```.zshenv``` and ```.bashrc``` add the following to the file:
```bash
export GITHUB_API_TOKEN=github_pat_tokenString
```
Note that you have to restart the terminal after adding the token for it to be available.

## Usage

### Add/Update packages with AutoTrust:

```PowerShell
autotrust add [<PROJECT>] package <PACKAGE_NAME> [options]
```

### Specific AutoTrust options:

Information about AutoTrust:

```PowerShell
autotrust add package -?, -h, --help
autotrust add package [PackageName] -?, -h, --help
```

More detailed output for AutoTrust:

```PowerShell
autotrust add package [PackageName] -ve, --verbosity <d|detailed|diag|diagnostic|n|normal|>
```

## Daily usage

`autotrust` is used prior to installing a dotnet package. It can be embed in your daily `dotnet` usage so you do not need to remember to run `autotrust` explicitly.

### For Windows PowerShell:

Run as administrator:

```PowerShell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine
```

Find profile file:

```PowerShell
echo $profile
```

Create or update profile file with command:

```PowerShell
New-Alias -Name dotnet -Value autotrust
```

Unblock file:

```PowerShell
Unblock-File -Path .\PathToProfileFile
```

Restart PowerShell.

### For Linux/Mac

```bash
alias dotnet='autotrust'
```

## Contributing

We welcome contributions to AutoTrust. If you'd like to contribute, please follow these steps:

1. Fork the repository
2. Create a new branch for your changes
3. Make the changes and commit them to your branch
4. Submit a pull request for review

### Working on the package

Creating a package:

```bash
dotnet pack
```

Run Project:

```bash
dotnet run add package PACKAGE VERSION
```

Testing the package locally:

```bash
dotnet tool install --global --add-source ./AutoTrust/nupkg AutoTrust
```

Uninstalling the package locally:

```bash
dotnet tool uninstall --global AutoTrust
```

## License

This project is licensed under the Apache License - see the [LICENSE](LICENSE) file for details.

<!-- ## API Flow

The flow of where the data is fetch is shown below.

![api flow](./Images/Api_flow.drawio.svg) -->
