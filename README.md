# BMI Calculator

[![BMI CI](https://github.com/SergiyKochenko/a00086200-bmi2026/actions/workflows/bmi_ci.yml/badge.svg)](https://github.com/SergiyKochenko/a00086200-bmi2026/actions/workflows/bmi_ci.yml)

A small ASP.NET Core Razor Pages app that calculates BMI from imperial
measurements. It targets .NET 10 and is hosted on Azure App Service.

## Live application

https://a00086200-bmi2026.azurewebsites.net

## Tests

Run the tests from the repository root:

```powershell
dotnet test bmi2024.sln --settings bmiUnitTestProject/coverlet.runsettings --collect:"XPlat Code Coverage"
```

The suite currently contains 26 tests and has 100% line and branch coverage for
the application C# code. Generated Razor markup and the process entry point are
not included in the coverage figure.

## Run locally

```powershell
dotnet run --project bmi2021/bmi2026.csproj
```

## Deployment

The `BMI CI` workflow runs automatically for pushes and pull requests to
`master`, and it can also be started manually. Pull requests build and test the
application; pushes and manual runs also deploy it to Azure App Service.
