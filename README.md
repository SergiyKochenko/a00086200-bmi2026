# BMI Calculator

[![BMI CI](https://github.com/SergiyKochenko/a00086200-bmi2026/actions/workflows/bmi_ci.yml/badge.svg)](https://github.com/SergiyKochenko/a00086200-bmi2026/actions/workflows/bmi_ci.yml)

An ASP.NET Core Razor Pages BMI calculator targeting .NET 10.

## Live application

https://a00086200-bmi2026.azurewebsites.net

## Tests

The test suite covers BMI calculations and categories, validation boundaries,
page models, error handling, and both Development and Production application
pipelines.

```powershell
dotnet test bmi2024.sln --settings bmiUnitTestProject/coverlet.runsettings --collect:"XPlat Code Coverage"
```

The application C# code has 100% line and branch coverage. Generated Razor
markup and the blocking process entry point are excluded from unit coverage.

## Deployment

Run the `BMI CI` workflow manually in GitHub Actions to build, test, publish,
and deploy the application to Azure App Service.
