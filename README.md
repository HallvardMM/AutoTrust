# AutoTrust

<p align="center">
  <img src="./Images/AutoTrust.png" alt="Auto Trust" width="250px"/>
</p>

## **Auto**matic software dependency auditing using **trust** criteria

AutoTrust is a Command Line Interface (CLI) tool for C# that fetches metadata about a NuGet package to help developers assess the package before installing it.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 7.0 or higher

## Usage

### Installing

Use the following command to install AutoTrust globally:

```PowerShell
dotnet tool install --global AutoTrust
```

### Add/Update packages with AutoTrust:

```PowerShell
autotrust add [PackageName]
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

## API Flow

The flow of where the data is fetch is shown below.

![api flow](./Images/Api_flow.drawio.svg)
