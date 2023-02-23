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

```bash
dotnet tool install --global AutoTrust
```

### Install packages with AutoTrust:

```bash
AutoTrust install [PackageName]
```

### Update packages with AutoTrust:

```bash
AutoTrust update [PackageName]
```

### Daily usage

`AutoTrust` is used prior to installing a dotnet package. It can be embed in your daily `dotnet` usage so you do not need to remember to run `AutoTrust` explicitly.

```bash
alias dotnet='AutoTrust'
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

Planned!:
![api flow](./Images/Api_flow.drawio.svg)
