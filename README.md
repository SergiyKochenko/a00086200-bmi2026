# BMI Calculator

[![BMI CI](https://github.com/SergiyKochenko/a00086200-bmi2026/actions/workflows/bmi_ci.yml/badge.svg)](https://github.com/SergiyKochenko/a00086200-bmi2026/actions/workflows/bmi_ci.yml)

The BMI Calculator is an ASP.NET Core Razor Pages application that calculates an
adult body mass index from imperial measurements. It was created as practice work
for the **TU Dublin Micro-credential in CI/CD (DevOps)**.

[View the deployed application](https://a00086200-bmi2026.azurewebsites.net)

![BMI Calculator home page with an empty measurement form](assets/screencapture-a00086200-bmi2026-azurewebsites-net-2026-09-29-22_23_59.png)

*Figure 1: The deployed calculator ready for user input.*

## Project information

| Role | Details |
| :--- | :--- |
| Student | [A00086200@myTUDublin.ie](mailto:A00086200@myTUDublin.ie) |
| Tutor | **Gary Clynch** |
| Programme | TU Dublin Micro-credential in CI/CD (DevOps) |

## Table of contents

1. [UX and UI](#ux-and-ui)
2. [Features](#features)
3. [Technologies used](#technologies-used)
4. [DevOps implementation](#devops-implementation)
5. [Testing](#testing)
6. [Issues and troubleshooting](#issues-and-troubleshooting)
7. [Deployment](#deployment)
8. [Credits](#credits)
9. [Acknowledgements](#acknowledgements)
10. [Disclaimer](#disclaimer)

## UX and UI

### Project goals

The application was built to demonstrate a complete CI/CD workflow around a
small web application. A user should be able to:

- enter weight in stone and additional pounds;
- enter height in feet and additional inches;
- receive a BMI value and category;
- understand validation errors without losing entered values;
- use the calculator on desktop and mobile devices.

The development workflow should also build, test and deploy the application in a
repeatable way whenever code is pushed to the `master` branch.

### Design

The interface uses a simple green, white and grey colour palette. The calculator
is presented in a centred card with related weight and height fields grouped
together. When a calculation succeeds, the result appears in a separate panel so
that it is easy to identify.

The layout changes to a single column on smaller screens. Form controls have
visible labels, keyboard focus styles and field-specific validation messages.

## Features

### BMI calculation

- Accepts imperial measurements using stone, pounds, feet and inches.
- Converts the measurements to kilograms and metres before calculating BMI.
- Displays the result to one decimal place.
- Classifies the result as Underweight, Normal, Overweight or Obese.

![BMI Calculator displaying an underweight result of 12.5](assets/screencapture-a00086200-bmi2026-azurewebsites-net-2026-09-29-22_29_14.png)

*Figure 2: A completed calculation showing the BMI value and category.*

### Form validation

- Weight in stone must be between 5 and 50.
- Additional pounds must be between 0 and 13.
- Height in feet must be between 4 and 7.
- Additional inches must be between 0 and 11.
- A result is displayed only when every field is valid.

![BMI Calculator showing validation errors for measurements above the permitted maximum values](assets/screencapture-a00086200-bmi2026-azurewebsites-net-2026-09-29-22_31_12.png)

*Figure 3: Client-side validation prevents out-of-range measurements from being submitted.*

### Navigation and supporting pages

- Home and brand links return to the calculator.
- The Privacy page explains how submitted measurements are handled.
- The Error page gives the user a route back to the calculator and displays a
  request reference when one is available.

### Responsive layout

The form, result panel, navigation and footer adapt to desktop and mobile screen
sizes. The mobile navigation can be expanded using the keyboard or pointer.

### Continuous integration and deployment

The `BMI CI` GitHub Actions workflow restores dependencies, builds the solution,
runs the automated test suite, publishes test results, packages the application
and deploys it to Azure App Service.

Pull requests run the build and test stages without deploying. Pushes to
`master`, and manually started workflow runs, also deploy the application.
The complete process is described in the
[DevOps implementation](#devops-implementation) section.

### Features left to implement

Possible future improvements include:

- an option to use metric measurements;
- a short explanation of each BMI category;
- automated browser accessibility checks in the CI workflow.

## Technologies used

- [ASP.NET Core Razor Pages](https://learn.microsoft.com/aspnet/core/razor-pages/) — web application framework.
- [C# and .NET 10](https://dotnet.microsoft.com/) — application and test code.
- HTML and CSS — page structure and responsive styling.
- [Bootstrap](https://getbootstrap.com/) — navigation and responsive utilities.
- [jQuery Validation](https://jqueryvalidation.org/) — client-side form validation.
- [MSTest](https://learn.microsoft.com/dotnet/core/testing/unit-testing-mstest-intro) — unit and application tests.
- [Coverlet](https://github.com/coverlet-coverage/coverlet) — code coverage collection.
- [GitHub Actions](https://docs.github.com/actions) — CI/CD automation.
- [Microsoft Azure App Service](https://azure.microsoft.com/products/app-service/) — application hosting.
- Git and GitHub — version control and repository hosting.

## DevOps implementation

This project uses GitHub Actions to provide a repeatable CI/CD pipeline from the
GitHub repository to Azure App Service. The workflow definition is stored in
[`.github/workflows/bmi_ci.yml`](.github/workflows/bmi_ci.yml), so changes to the
pipeline are version-controlled alongside the application.

```mermaid
flowchart LR
    A[Push, pull request or manual run] --> B[Restore dependencies]
    B --> C[Build in Release mode]
    C --> D[Run 26 tests with coverage]
    D --> E[Publish test results]
    E --> F[Publish application package]
    F --> G{Pull request?}
    G -- Yes --> H[Finish without deployment]
    G -- No --> I[Deploy to Azure App Service]
```

### Workflow triggers

| Event | Build and test | Deploy to Azure |
| :--- | :---: | :---: |
| Pull request targeting `master` | Yes | No |
| Push to `master` | Yes | Yes |
| Manual workflow run | Yes | Yes |

The Release build and all 26 automated tests must pass before the application is
published and deployed. Test results appear as an **MS Tests** check in GitHub,
while the workflow badge at the top of this README shows the current pipeline
status. The test suite provides 100% line and branch coverage for the selected
application code.

### Deployment secret

Azure authentication is provided through the GitHub Actions repository secret
`AZURE_WEBAPP_PUBLISH_PROFILE`. GitHub encrypts the saved value and supplies it
to the deployment step only while the workflow is running. The publish profile
is not stored in the repository, workflow file or README.

![GitHub Actions repository secret configuration showing AZURE_WEBAPP_PUBLISH_PROFILE](assets/screencapture-github-SergiyKochenko-a00086200-bmi2026-settings-secrets-actions-2026-09-30-21_23_05.png)

*Figure 4: The GitHub Actions repository secret used to authenticate the Azure deployment. The protected value is not displayed.*

## Testing

### Automated tests

Run the complete test suite from the repository root:

```powershell
dotnet test bmi2024.sln `
  --settings bmiUnitTestProject/coverlet.runsettings `
  --collect "XPlat Code Coverage"
```

The suite contains **26 passing tests**. Coverage for the application C# code is:

| Measurement | Result |
| :--- | :--- |
| Line coverage | 100% — 41/41 lines |
| Branch coverage | 100% — 12/12 branches |

Generated Razor code and the blocking process entry point are not included in
the coverage calculation.

![HTML code coverage report showing 100 percent line and branch coverage](assets/screencapture-file-C-Users-Sergiy-Desktop-bmi2026-artifacts-coverage-report-index-html-2026-09-29-22_27_13.png)

*Figure 5: The generated HTML coverage report, including class-level results.*

### Test cases

| Area | Test | Expected result | Status |
| :--- | :--- | :--- | :---: |
| Calculation | Enter 12 stone, 0 pounds, 5 feet and 10 inches | BMI is displayed as 24.1 with category Normal | Pass |
| Categories | Exercise values in all four BMI ranges | Correct category is returned for each value | Pass |
| Boundaries | Use the minimum and maximum permitted measurements | Values are accepted | Pass |
| Validation | Use values outside each permitted range | A field-specific error is returned | Pass |
| Required fields | Submit without measurements | Four required-field errors are returned | Pass |
| Page model | Submit a valid model | Result display is enabled | Pass |
| Page model | Submit an invalid model | Result display remains hidden | Pass |
| Error handling | Load an error page with and without an active request | Correct request reference is selected | Pass |
| Application | Load the home page in Development and Production | HTTP 200 and expected page content | Pass |
| Privacy | Load `/Privacy` | HTTP 200 and privacy information | Pass |

![BMI Calculator accepting the maximum permitted values and displaying the calculated result](assets/screencapture-a00086200-bmi2026-azurewebsites-net-2026-09-29-22_32_09.png)

*Figure 6: Boundary testing with the maximum valid value for every measurement field.*

### Manual testing

The deployed and local applications were checked at desktop and mobile viewport
sizes. The following were verified:

- the calculator accepts valid input and returns the expected result;
- empty and out-of-range fields show useful validation messages;
- Home, Privacy and brand links work;
- the mobile navigation opens and closes correctly;
- the Privacy page contains the expected information;
- no browser console errors occur during the tested journeys.

### Code quality checks

- `dotnet format` completes successfully.
- The solution builds in Release configuration.
- The current NuGet vulnerability audit reports no vulnerable packages.
- The repository contains no embedded Azure publishing credentials.

## Issues and troubleshooting

### Issues resolved

| Issue | Resolution |
| :--- | :--- |
| Home and brand links had empty destinations | Both links now target the routed BMI page. |
| The original Privacy page contained placeholder text | It was replaced with application-specific privacy information. |
| Results depended on checking the raw HTTP method in the Razor view | Result visibility is now controlled by validated page-model state. |
| Old deployment templates referred to unrelated Azure resources | The obsolete templates were removed. |
| Azure rejected the publish profile during the first workflow run | SCM publishing was enabled, the exposed credential was rotated and the refreshed profile was stored as a GitHub Actions secret. FTP publishing remains disabled. |

### Known bugs

No known functional bugs remain at the time of this README update.

## Deployment

### Azure deployment

The production application is hosted at:

<https://a00086200-bmi2026.azurewebsites.net>

The GitHub Actions workflow is stored in
[`.github/workflows/bmi_ci.yml`](.github/workflows/bmi_ci.yml). It uses the
repository secret `AZURE_WEBAPP_PUBLISH_PROFILE` to authenticate the deployment
without storing publishing credentials in source control.

To configure the same deployment workflow for another Azure App Service:

1. Create an Azure App Service that supports the project's .NET version.
2. Download the App Service publish profile from the Azure portal.
3. Open the GitHub repository and go to **Settings → Secrets and variables → Actions**.
4. Create a repository secret named `AZURE_WEBAPP_PUBLISH_PROFILE`.
5. Paste the complete publish profile into the secret value.
6. Update `AZURE_WEBAPP_NAME` in `bmi_ci.yml` if the App Service name is different.
7. Run the workflow manually or push a commit to `master`.

Publish profiles contain credentials and must never be committed to the
repository or included in documentation.

### Run locally

The .NET 10 SDK is required.

```powershell
git clone https://github.com/SergiyKochenko/a00086200-bmi2026.git
cd a00086200-bmi2026
dotnet restore bmi2024.sln
dotnet run --project bmi2021/bmi2026.csproj
```

Use the local address printed by `dotnet run` to open the application.

### Fork the repository

1. Sign in to GitHub and open the
   [project repository](https://github.com/SergiyKochenko/a00086200-bmi2026).
2. Select **Fork** at the top of the page.
3. Choose the account or organisation that should own the fork.
4. Select **Create fork**.

### Clone an existing fork

1. Open the fork on GitHub.
2. Select **Code** and copy the HTTPS, SSH or GitHub CLI address.
3. Run `git clone` followed by the copied address.
4. Open the newly created project directory in an editor.

## Credits

The application was written specifically for this practice project. The
third-party libraries stored under `wwwroot/lib` retain their original licence
files. Links to the main frameworks and tools are provided in the
[Technologies used](#technologies-used) section.

## Acknowledgements

Thanks to **Gary Clynch** for tutoring and guidance during the TU Dublin
Micro-credential in CI/CD (DevOps).

## Disclaimer

This application was created for educational practice and is not intended for
commercial use. BMI is a general screening measure and is not a medical
diagnosis. Anyone concerned about their health should seek advice from a
qualified healthcare professional.
